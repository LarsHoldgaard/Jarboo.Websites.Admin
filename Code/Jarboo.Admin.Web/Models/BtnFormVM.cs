using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Models
{
    public class BtnFormVM
    {
        public BtnFormVM()
        {
            Small = true;
        }

        public int Id { get; set; }
        public ActionResult Action { get; set; }
        public bool Small { get; set; }
        public string Text { get; set; }
        public string Style { get; set; }
        public string Icon { get; set; }

        public static BtnFormVM Delete(Action<BtnFormVM> init = null)
        {
            var form = new BtnFormVM()
                       {
                           Text = "Delete",
                           Style = "danger",
                           Icon = "remove-sign"
                       };

            if (init != null)
            {
                init(form);
            }

            return form;
        }
    }
}