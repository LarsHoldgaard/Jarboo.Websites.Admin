using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Includes
{
    public class Include<T>
    {
        public static Include<T> None = new Include<T>();

        public virtual IQueryable<T> Execute(IQueryable<T> query)
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

    public static class IncludeExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, Include<T> include)
        {
            return include.Execute(query);
        }
    }
}
