using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.Filters;

namespace Jarboo.Admin.Web.Models.Task
{
    public class TasksListViewModel
    {
        public bool ShowProject { get; set; }
        public TaskFilter TaskFilter { get; set; }
        public TaskSorting Sorting { get; set; }
    }
}