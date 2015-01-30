using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Filters;
using Jarboo.Admin.BL.Includes;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        [Inject]
        public IProjectService ProjectService { get; set; }
        [Inject]
        public ICustomerService CustomerService { get; set; }

        public virtual ActionResult Index()
        {
            if (UserCustomerId != null)
            {
                return RedirectToAction(MVC.Customers.View(UserCustomerId));
            } 
            else if (UserEmployeeId != null)
            {
                return RedirectToAction(MVC.Tasks.NextTask());
            }

            return RedirectToAction(MVC.Home.Dashboard());
        }

        public virtual ActionResult Dashboard()
        {
            ViewBag.Customers = CustomerService.GetAll(Query.ForCustomer().Include(x => x.Projects()));

            return View();
        }
    }
}