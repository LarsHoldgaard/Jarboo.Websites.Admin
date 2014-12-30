using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        public virtual ActionResult Index()
        {
            var errors = TempData[TempErrorsKey] as List<string>;
            if (errors != null && errors.Count != 0)
            {
                ViewBag.Error = errors[0];
            }
            return View();
        }
	}
}