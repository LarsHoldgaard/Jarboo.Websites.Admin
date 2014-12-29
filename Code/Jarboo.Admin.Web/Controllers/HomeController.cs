using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Services;

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
            ViewBag.Customers = CustomerService.GetAll();

            return View();
        }
	}
}