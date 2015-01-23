using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    public class Filter<T>
        where T : BaseEntity
    {
        internal static Filter<T> None = new Filter<T>();

        public virtual IQueryable<T> Execute(IQueryable<T> query)
        {
            return query;
        }
    }

    public static class FilterExtensions
    {
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> query, Filter<T> filter)
            where T : BaseEntity
        {
            return filter.Execute(query);
        }
    }
}
