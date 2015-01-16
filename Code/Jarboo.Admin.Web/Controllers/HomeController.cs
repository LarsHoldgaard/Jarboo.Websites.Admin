﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            ViewBag.Customers = CustomerService.GetAllEx(Include.ForCustomer().Projects(), BL.Filters.Filter.ForCustomer());

            return View();
        }
	}
}