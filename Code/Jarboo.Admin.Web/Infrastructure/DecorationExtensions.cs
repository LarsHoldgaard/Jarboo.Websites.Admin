using System.Collections.Generic;
using System.Linq;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Account;
using Jarboo.Admin.Web.Models.Report;
using Jarboo.Admin.Web.Models.Task;
using Jarboo.Admin.BL;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class DecorationExtensions
    {
        public static TaskViewModel Decorate(this Task task)
        {
            return task.MapTo<TaskViewModel>();
        }
        public static UserViewModel Decorate(this User task)
        {
            return task.MapTo<UserViewModel>();
        }

        public static IEnumerable<TaskViewModel> Decorate(this IEnumerable<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<TaskViewModel>());
        }

        public static List<TaskViewModel> Decorate(this List<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<TaskViewModel>()).ToList();
        }

        public static ReportViewModel DecorateReport(this Task task)
        {
            return task.MapTo<ReportViewModel>();
        }

        public static IEnumerable<ReportViewModel> DecorateReport(this IEnumerable<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<ReportViewModel>());
        }

        public static List<ReportViewModel> DecorateReport(this List<Task> tasks)
        {
            return tasks.Select(x => x.MapTo<ReportViewModel>()).ToList();
        }

        public static IEnumerable<TVM> Decorate<T, TVM>(this IEnumerable<T> entities)
            where T : class
            where TVM : class
        {
            return entities.Select(x => x.MapTo<TVM>()).ToList();
        }
    }
}