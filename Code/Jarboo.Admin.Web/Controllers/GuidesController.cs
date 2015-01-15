using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class GuidesController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
	}
}