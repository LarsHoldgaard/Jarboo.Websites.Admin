﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Sorters;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL
{
    public interface IQuery
    {
        int? PageSize { get; set; }
        int? PageNumber { get; set; }
    }

    public interface IQuery<TEntity, out TInclude, out TFilter, out TSorter> : IQuery
        where TEntity : IBaseEntity
        where TFilter : Filter<TEntity>, new()
        where TInclude : Include<TEntity>, new()
        where TSorter : Sorter<TEntity>, new()
    {
        TFilter Filter { get; }
        TInclude Include { get; }
        TSorter Sorter { get; }
    }

    public class Query<TEntity, TInclude, TFilter, TSorter> : IQuery<TEntity, TInclude, TFilter, TSorter>
        where TEntity : IBaseEntity
        where TFilter : Filter<TEntity>, new()
        where TInclude : Include<TEntity>, new()
        where TSorter : Sorter<TEntity>, new()
    {
        public TFilter Filter { get; private set; }
        public TInclude Include { get; private set; }
        public TSorter Sorter { get; private set; }

        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public Query(TFilter filter)
        {
            Filter = filter ?? new TFilter();
            Include = new TInclude();
            Sorter = new TSorter();
        }

        public override string ToString()
        {
            var type = GetType();

            var list = new List<string>();

            if (Filter != null) list.Add(Filter.ToString());
            if (Include != null) list.Add(Include.ToString());
            if (Sorter != null) list.Add(Sorter.ToString());

            foreach (var prop in type.GetProperties())
            {
                if (prop.Name == "Filter" || prop.Name == "Include" || prop.Name == "Sorter") continue;

                var value = GetValue(prop.GetValue(this));

                if (String.IsNullOrEmpty(value)) continue;

                list.Add(String.Format("{0}={1}", prop.Name, value));
            }

            var res = String.Join("&", list.ToArray());
            return res;
        }

        private string GetValue(object value)
        {
            if (value == null)
            {
                return null;
            }

            var type = value.GetType();

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return String.Join(",", (from object val in (value as System.Collections.IList) select GetValue(val)).ToArray());
                }
                else
                {
                    throw new Exception("Special Generic Type not implemented");
    }
            }
            else
            {
                if (type.IsEnum)
                {
                    return ((int)value).ToString();
                }

                return value.ToString();
            }
        }
    }

    public static class Query
    {
        public static IQuery<Customer, CustomerInclude, CustomerFilter, CustomerSorter> ForCustomer(CustomerFilter filter = null)
        {
            return new Query<Customer, CustomerInclude, CustomerFilter, CustomerSorter>(filter);
        }

        public static IQuery<Project, ProjectInclude, ProjectFilter, ProjectSorter> ForProject(ProjectFilter filter = null)
        {
            return new Query<Project, ProjectInclude, ProjectFilter, ProjectSorter>(filter);
        }

        public static IQuery<Jarboo.Admin.DAL.Entities.Task, TaskInclude, TaskFilter, TaskSorter> ForTask(TaskFilter filter = null)
        {
            return new Query<Jarboo.Admin.DAL.Entities.Task, TaskInclude, TaskFilter, TaskSorter>(filter);
        }

        public static IQuery<Jarboo.Admin.DAL.Entities.Task, ReportInclude, ReportFilter, ReportSorter> ForReport(ReportFilter filter = null)
        {
            return new Query<Jarboo.Admin.DAL.Entities.Task, ReportInclude, ReportFilter, ReportSorter>(filter);
        }

        public static IQuery<Employee, EmployeeInclude, EmployeeFilter, EmployeeSorter> ForEmployee(EmployeeFilter filter = null)
        {
            return new Query<Employee, EmployeeInclude, EmployeeFilter, EmployeeSorter>(filter);
        }

        public static IQuery<Documentation, DocumentationInclude, DocumentationFilter, DocumentationSorter> ForDocumentation(DocumentationFilter filter = null)
        {
            return new Query<Documentation, DocumentationInclude, DocumentationFilter, DocumentationSorter>(filter);
        }

        public static IQuery<User, UserInclude, UserFilter, UserSorter> ForUser(UserFilter filter = null)
        {
            return new Query<User, UserInclude, UserFilter, UserSorter>(filter);
        }

        public static IQuery<SpentTime, SpentTimeInclude, SpentTimeFilter, SpentTimeSorter> ForSpentTime(SpentTimeFilter filter = null)
        {
            return new Query<SpentTime, SpentTimeInclude, SpentTimeFilter, SpentTimeSorter>(filter);
        }

        public static IQuery<Question, QuestionInclude, QuestionFilter, QuestionSorter> ForQuestion(QuestionFilter filter = null)
        {
            return new Query<Question, QuestionInclude, QuestionFilter, QuestionSorter>(filter);
        }

        public static IQuery<Comment, CommentInclude, CommentFilter, CommentSorter> ForComment(CommentFilter filter = null)
        {
            return new Query<Comment, CommentInclude, CommentFilter, CommentSorter>(filter);
        }

        public static IQuery<Answer, AnswerInclude, AnswerFilter, AnswerSorter> ForAnswer(AnswerFilter filter = null)
        {
            return new Query<Answer, AnswerInclude, AnswerFilter, AnswerSorter>(filter);
        }

        public static IQuery<Quiz, QuizInclude, QuizFilter, QuizSorter> ForQuiz(QuizFilter filter = null)
        {
            return new Query<Quiz, QuizInclude, QuizFilter, QuizSorter>(filter);
        }
    }

    public static class QueryExtensions
    {
        public static T WithPaging<T>(this T query, int pageSize, int pageNumber)
            where T : IQuery
        {
            query.PageSize = pageSize;
            query.PageNumber = pageNumber;

            return query;
        }
        public static PagedData<T> Paginate<T>(this IQueryable<T> query, IQuery paging) where T : class, IBaseEntity
        {
            if (!paging.PageNumber.HasValue || !paging.PageSize.HasValue)
            {
                return PagedData.AllOnOnePage(query);
            }

            if (!query.IsOrdered())
            {
                query = query.OrderBy(x => x.DateCreated);
            }

            return PagedData.Create(paging.PageSize.Value, paging.PageNumber.Value, query);
        }

        public static async Task<PagedData<T>> PaginateAsync<T>(this IQueryable<T> query, IQuery paging) where T : class, IBaseEntity
        {
            if (!paging.PageNumber.HasValue || !paging.PageSize.HasValue)
            {
                return await PagedData.AllOnOnePageAsync(query);
            }

            if (!query.IsOrdered())
            {
                query = query.OrderBy(x => x.DateCreated);
            }

            return await PagedData.CreateAsync(paging.PageSize.Value, paging.PageNumber.Value, query);
        }

        public static IQuery<TEntity, TInclude, TFilter, TSorter> Filter<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TFilter> action)
            where TEntity : IBaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Filter);
            return query;
        }

        public static IQuery<TEntity, TInclude, TFilter, TSorter> Include<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TInclude> action)
            where TEntity : IBaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Include);
            return query;
        }

        public static IQuery<TEntity, TInclude, TFilter, TSorter> Sort<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TSorter> action)
            where TEntity : IBaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Sorter);
            return query;
        }

        public static PagedData<TEntity> ApplyTo<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, IQueryable<TEntity> data, Func<IQueryable<TEntity>, IQueryable<TEntity>> securityFilter = null)
            where TEntity : class, IBaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            var filteredData = data.Include(query.Include).FilterBy(query.Filter);
            if (securityFilter != null)
            {
                filteredData = securityFilter(filteredData);
            }
            return filteredData.SortBy(query.Sorter).Paginate(query);
        }

        public static async Task<PagedData<TEntity>> ApplyToAsync<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, IQueryable<TEntity> data, Func<IQueryable<TEntity>, IQueryable<TEntity>> securityFilter = null)
            where TEntity : class, IBaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            var filteredData = data.Include(query.Include).FilterBy(query.Filter);
            if (securityFilter != null)
            {
                filteredData = securityFilter(filteredData);
            }
            return await filteredData.SortBy(query.Sorter).PaginateAsync(query);
        }
    }
}
