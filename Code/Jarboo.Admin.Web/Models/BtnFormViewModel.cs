using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Models
{
    public class BtnFormViewModel
    {
        public BtnFormViewModel()
        {
            Small = true;
        }

        public int Id { get; set; }
        public ActionResult Action { get; set; }
        public bool Small { get; set; }
        public string Text { get; set; }
        public string Style { get; set; }
        public string Icon { get; set; }

        public static BtnFormViewModel Delete(Action<BtnFormViewModel> init = null)
        {
            var form = new BtnFormViewModel()
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