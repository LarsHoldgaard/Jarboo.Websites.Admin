using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class CustomerExtensions
    {
        public static Customer ByIdMust(this IQueryable<Customer> query, int customerId)
        {
            var customer = query.ById(customerId);
            if (customer == null)
            {
                throw new Exception("Couldn't find customer " + customerId);
            }
            return customer;
        }
        public static Customer ById(this IQueryable<Customer> query, int customerId)
        {
            return query.FirstOrDefault(x => x.CustomerId == customerId);
        }

        public static Customer ByProjectMust(this IQueryable<Customer> query, int projectId)
        {
            var customer = ByProject(query, projectId);
            if (customer == null)
            {
                throw new Exception("Couldn't find customer for project " + projectId);
            }

            return customer;
        }
        public static Customer ByProject(this IQueryable<Customer> query, int projectId)
        {
            return query.FirstOrDefault(x => x.Projects.Any(y => y.ProjectId == projectId));
        }
    }
}
