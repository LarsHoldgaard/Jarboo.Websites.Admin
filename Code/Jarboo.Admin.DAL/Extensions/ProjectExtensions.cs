using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class ProjectExtensions
    {
        public static Project ByIdMust(this IQueryable<Project> query, int projectId)
        {
            var employee = query.ById(projectId);
            if (employee == null)
            {
                throw new Exception("Couldn't find project " + projectId);
            }
            return employee;
        }
        public static Project ById(this IQueryable<Project> query, int projectId)
        {
            return query.FirstOrDefault(x => x.ProjectId == projectId);
        }
    }
}
