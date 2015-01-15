using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Project
{
    public class ProjectsListViewModel
    {
        public IEnumerable<DAL.Entities.Project> Projects { get; set; }
        public bool ShowCustomer { get; set; }
    }
}