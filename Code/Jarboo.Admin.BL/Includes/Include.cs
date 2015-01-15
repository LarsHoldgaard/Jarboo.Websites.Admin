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
    }

    public static class Include
    {
        public static CustomerInclude ForCustomer()
        {
            return new CustomerInclude();
        }
        public static ProjectInclude ForProject()
        {
            return new ProjectInclude();
        }
        public static TaskInclude ForTask()
        {
            return new TaskInclude();
        }
        public static EmployeeInclude ForEmployee()
        {
            return new EmployeeInclude();
        }
        public static DocumentationInclude ForDocumentation()
        {
            return new DocumentationInclude();
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
