using System;
using System.Data.Entity;
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
            var project = query.ById(projectId);
            if (project == null)
            {
                throw new Exception("Couldn't find project " + projectId);
            }
            return project;
        }
        public static Project ById(this IQueryable<Project> query, int projectId)
        {
            return query.FirstOrDefault(x => x.ProjectId == projectId);
        }
        public static async Task<Project> ByIdAsync(this IQueryable<Project> query, int projectId)
        {
            return await query.FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }
    }
}
