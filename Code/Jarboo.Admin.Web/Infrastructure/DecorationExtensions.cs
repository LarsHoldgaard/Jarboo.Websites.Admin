using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Task;
using Jarboo.Admin.BL;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class DecorationExtensions
    {
        public static TaskVM Decorate(this Task task)
        {
            return task.MapTo<TaskVM>();
        }
        public static IEnumerable<TaskVM> Decorate(this IEnumerable<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<TaskVM>());
        }
        public static List<TaskVM> Decorate(this List<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<TaskVM>()).ToList();
        }
    }
}