using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Sorters
{
    public enum SortDirection
    {
        Ascendant,
        Descendant
    }

    public class Sorter<T>
    {
        public virtual IQueryable<T> Sort(IQueryable<T> query)
        {
            return query;
        }
    }

    public static class SortExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, Sorter<T> sorter)
        {
            return sorter.Sort(query);
        }
        public static IQueryable<T> SortBy<T, TKey>(this IQueryable<T> query, SortDirection direction, Expression<Func<T, TKey>> keySelector)
        {
            if (direction == SortDirection.Ascendant)
            {
                return query.OrderBy(keySelector);
            }
            else
            {
                return query.OrderByDescending(keySelector);
            }
        }
        public static IEnumerable<T> SortBy<T, TKey>(this IEnumerable<T> query, SortDirection direction, Func<T, TKey> keySelector)
        {
            if (direction == SortDirection.Ascendant)
            {
                return query.OrderBy(keySelector);
            }
            else
            {
                return query.OrderByDescending(keySelector);
            }
        }
    }
}
