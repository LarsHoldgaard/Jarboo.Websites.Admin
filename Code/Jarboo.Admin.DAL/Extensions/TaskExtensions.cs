using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.DAL.Extensions
{
    public static class TaskExtensions
    {
        public static Task ByIdMust(this IQueryable<Task> query, int taskId)
        {
            var task = query.ById(taskId);
            if (task == null)
            {
                throw new Exception("Couldn't find task " + taskId);
            }
            return task;
        }
        public static Task ById(this IQueryable<Task> query, int taskId)
        {
            return query.FirstOrDefault(x => x.TaskId == taskId);
        }
    }
}
