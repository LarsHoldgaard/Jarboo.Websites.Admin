using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Models
{
    public class DeleteBtnVM
    {
        public int Id { get; set; }
        public ActionResult Action { get; set; }
        public bool Small { get; set; }
    }
}