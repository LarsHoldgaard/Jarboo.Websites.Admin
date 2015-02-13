using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Employee
{
    public class EmployeeListViewModel
    {
        public string Query { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public bool? IsHired { get; set; }
        public IEnumerable<Jarboo.Admin.DAL.Entities.Employee> Employees { get; set; }
    }
}