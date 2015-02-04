using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Services;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class SpentTimeController : BaseController
    {
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        //
        // GET: /SpentTime/
        public virtual ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public virtual ActionResult GroupedList(SpentTimeFilter spentTimeFilter = null)
        {
            spentTimeFilter = spentTimeFilter ?? new SpentTimeFilter();
            var items = SpentTimeService.GetAll(Query.ForSpentTime(spentTimeFilter));

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee().Include(x => x.Positions())), "EmployeeId", "FullName");
            return this.View(items);
        }
	}
}