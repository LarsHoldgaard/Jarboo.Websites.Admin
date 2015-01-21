using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class FilterBase
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }

    public class Filter<T> : FilterBase
        where T : BaseEntity
    {
        internal static Filter<T> None = new Filter<T>();

        public virtual PagedData<T> Execute(IQueryable<T> query)
        {
            if (!PageNumber.HasValue || !PageSize.HasValue)
            {
                return PagedData.AllOnOnePage(query);
            }

            return PagedData.Create(PageSize.Value, PageNumber.Value, query);
        }
    }

    public static class Filter
    {
        public static CustomerFilter ForCustomer()
        {
            return new CustomerFilter();
        }
        public static ProjectFilter ForProject()
        {
            return new ProjectFilter();
        }
        public static TaskFilter ForTask()
        {
            return new TaskFilter();
        }
        public static EmployeeFilter ForEmployee()
        {
            return new EmployeeFilter();
        }
        public static DocumentationFilter ForDocumentation()
        {
            return new DocumentationFilter();
        }
    }

    public static class FilterExtensions
    {
        public static PagedData<T> Filter<T>(this IQueryable<T> query, Filter<T> filter)
            where T : BaseEntity
        {
            return filter.Execute(query);
        }

        public static T WithPaging<T>(this T filter, int pageSize, int pageNumber)
            where T: FilterBase
        {
            filter.PageSize = pageSize;
            filter.PageNumber = pageNumber;

            return filter;
        }
    }
}
