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

        public override string ToString()
        {
            var type = GetType();

            var list = new List<string>();

            foreach (var prop in type.GetProperties())
            {
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
