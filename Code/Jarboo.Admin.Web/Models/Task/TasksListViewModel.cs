using System.Collections.Generic;
using Jarboo.Admin.BL.Filters;

namespace Jarboo.Admin.Web.Models.Task
{
    public class TasksListViewModel
    {
        public bool ShowProject { get; set; }
        public TaskFilter TaskFilter { get; set; }
        public TaskSorting Sorting { get; set; }
        public List<DAL.Entities.Task> Tasks { get; set; }
    }
}