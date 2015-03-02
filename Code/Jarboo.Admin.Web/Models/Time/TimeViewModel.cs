using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Models.Time
{
    public class TimeViewModel
    {
        public TimeViewModel()
        {
            Date = DateTime.Now.Date;
        }

        public Position? Roles { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public decimal? Hours { get; set; }
        public decimal? Price { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
    }
}