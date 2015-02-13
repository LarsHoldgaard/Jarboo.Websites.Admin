using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Services;

using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class SpentTimeController : BaseController
    {
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        public virtual ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public virtual ActionResult GroupedList(SpentTimeFilter spentTimeFilter = null)
        {
            spentTimeFilter = (spentTimeFilter ?? new SpentTimeFilter()).ByAccepted(true);
            var items = SpentTimeService.GetAll(Query.ForSpentTime(spentTimeFilter));

            ViewBag.EmployeesList = new SelectList(EmployeeService.GetAll(Query.ForEmployee().Include(x => x.Positions())), "EmployeeId", "FullName");
            return this.View(items);
        }

        [ChildActionOnly]
        public virtual ActionResult List(SpentTimeFilter spentTimeFilter = null)
        {
            spentTimeFilter = spentTimeFilter ?? new SpentTimeFilter();
            var items = SpentTimeService.GetAll(Query.ForSpentTime(spentTimeFilter).Include(x => x.Task().Project().Customer().Employee()));
            return this.View(items);
        }

        [HttpPost]
        public virtual ActionResult Accept(int id, string returnUrl)
        {
            var result = this.RedirectToLocalUrl(returnUrl, MVC.SpentTime.Index());
            return Handle(id, SpentTimeService.Accept, result, result, "Accepted");
        }

        [HttpPost]
        public virtual ActionResult Deny(int id, string returnUrl)
        {
            var result = this.RedirectToLocalUrl(returnUrl, MVC.SpentTime.Index());
            return Handle(id, SpentTimeService.Deny, result, result, "Denied");
        }
	}
}