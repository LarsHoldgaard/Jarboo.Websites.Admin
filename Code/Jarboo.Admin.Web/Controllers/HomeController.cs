using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Account;
using Jarboo.Admin.Web.Infrastructure;

using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        [Inject]
        public IProjectService ProjectService { get; set; }
        [Inject]
        public ICustomerService CustomerService { get; set; }
        [Inject]
        public ISpentTimeService SpentTimeService { get; set; }
        [Inject]
        public ITaskService TaskService { get; set; }

        public virtual ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(MVC.Accounts.Index());
            }
            else if (UserEmployeeId != null)
            {
                return RedirectToAction(MVC.Tasks.NextTask());
            }

            return RedirectToAction(MVC.Home.Dashboard());
        }

        public virtual ActionResult Dashboard()
        {
            if (this.Can(MVC.Home.Name, "OverallIncoming"))
            {
                ViewBag.IncomingByDate = SpentTimeService.GetAll(Query.ForSpentTime()
                    .Filter(x => x.ByAccepted(true).ByFromDate(DateTime.Now.AddMonths(-1))))
                    .Data
                    .GroupBy(x => x.Date.Date).OrderBy(x => x.Key)
                    .Select(x => x.Aggregate(0d, (a, y) => a + (double)(y.Price.GetValueOrDefault() * y.Hours.GetValueOrDefault())));
            }

            if (this.Can(MVC.Home.Name, "TaskStats"))
            {
                ViewBag.TasksAfterDeadline = TaskService.GetAll(Query.ForTask().Filter(x => x.WithExceededDeadline())).Data.Count;
                ViewBag.TasksAfterFollowUp = TaskService.GetAll(Query.ForTask().Filter(x => x.WithExceededFollowUp())).Data.Count;
            }

            return View();
        }

        public virtual ActionResult Landing()
        {
            return this.View(new LandingViewModel());
        }

        [HttpPost]
        public virtual ActionResult Landing(LandingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PreserveModelState();
                return RedirectToAction(MVC.Home.Landing());
            }

            ViewBag.ReturnUrl = "";
            return View(MVC.Accounts.Views.Register, new UserRegister()
                            {
                                Email = model.Email,
                                ConfirmEmail = model.Email,
                                Password = model.Password
                            });
        }
    }
}