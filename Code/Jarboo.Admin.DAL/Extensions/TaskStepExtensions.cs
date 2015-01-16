using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class TaskStepExtensions
    {
        public static IQueryable<TaskStep> ForEmployee(this IQueryable<TaskStep> query, int employeeId)
        {
            return query.Where(x => x.EmployeeId == employeeId);
        }

        public static IQueryable<TaskStep> NotDone(this IQueryable<TaskStep> query)
        {
            return query.Where(x => !x.DateEnd.HasValue);
        }
    }
}
