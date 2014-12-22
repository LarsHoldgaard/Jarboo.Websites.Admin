using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.DAL;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class CustomersController : Controller
    {
        [Inject]
        public ICustomerService CustomerService { get; set; }

        // GET: /Customer/
        public virtual ActionResult Index()
        {
            return View(CustomerService.GetAll());
        }

        // GET: /Customer/View/5
        public virtual ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = CustomerService.GetById(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
    }
}
