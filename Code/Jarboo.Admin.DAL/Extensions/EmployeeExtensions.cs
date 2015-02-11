using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class EmployeeExtensions
    {
        public static Employee ByIdMust(this IQueryable<Employee> query, int employeeId)
        {
            var employee = query.ById(employeeId);
            if (employee == null)
            {
                throw new Exception("Couldn't find employee " + employeeId);
            }
            return employee;
        }

        public static Employee ById(this IQueryable<Employee> query, int employeeId)
        {
            return query.FirstOrDefault(x => x.EmployeeId == employeeId);
        }

        public static async Task<Employee> ByIdAsync(this IQueryable<Employee> query, int employeeId)
        {
            return await query.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
        }

        public static Employee ByUserId(this IQueryable<Employee> query, string userId)
        {
            return query.FirstOrDefault(x => x.User.Id == userId);
        }
    }
}
