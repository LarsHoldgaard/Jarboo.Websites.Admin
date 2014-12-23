using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Task
{
    public class TasksListViewModel
    {
        public List<DAL.Entities.Task> Tasks { get; set; }
        public bool ShowProject { get; set; }
    }
}