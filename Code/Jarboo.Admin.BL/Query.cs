﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        where TEntity : BaseEntity
        where TFilter : Filter<TEntity>, new()
        where TInclude : Include<TEntity>, new()
        where TSorter : Sorter<TEntity>, new()
    {
        TFilter Filter { get; }
        TInclude Include { get; }
        TSorter Sorter { get; }
    }

    public class Query<TEntity, TInclude, TFilter, TSorter> : IQuery<TEntity, TInclude, TFilter, TSorter>
        where TEntity: BaseEntity
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
        public static IQuery<Employee, EmployeeInclude, EmployeeFilter, EmployeeSorter> ForEmployee(EmployeeFilter filter = null)
        {
            return new Query<Employee, EmployeeInclude, EmployeeFilter, EmployeeSorter>(filter);
        }
        public static IQuery<Documentation, DocumentationInclude, DocumentationFilter, DocumentationSorter> ForDocumentation(DocumentationFilter filter = null)
        {
            return new Query<Documentation, DocumentationInclude, DocumentationFilter, DocumentationSorter>(filter);
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
        public static PagedData<T> Paginate<T>(this IQueryable<T> query, IQuery paging)
            where T : BaseEntity
        {
            if (!paging.PageNumber.HasValue || !paging.PageSize.HasValue)
            {
                return PagedData.AllOnOnePage(query);
            }

            return PagedData.Create(paging.PageSize.Value, paging.PageNumber.Value, query);
        }

        public static IQuery<TEntity, TInclude, TFilter, TSorter> Filter<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TFilter> action)
            where TEntity : BaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Filter);
            return query;
        }
        public static IQuery<TEntity, TInclude, TFilter, TSorter> Include<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TInclude> action)
            where TEntity : BaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Include);
            return query;
        }
        public static IQuery<TEntity, TInclude, TFilter, TSorter> Sort<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, Action<TSorter> action)
            where TEntity : BaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            action(query.Sorter);
            return query;
        }

        public static PagedData<TEntity> ApplyTo<TEntity, TInclude, TFilter, TSorter>(this IQuery<TEntity, TInclude, TFilter, TSorter> query, IQueryable<TEntity> data)
            where TEntity : BaseEntity
            where TFilter : Filter<TEntity>, new()
            where TInclude : Include<TEntity>, new()
            where TSorter : Sorter<TEntity>, new()
        {
            return data.Include(query.Include).FilterBy(query.Filter).SortBy(query.Sorter).Paginate(query);
        }
    }
}
