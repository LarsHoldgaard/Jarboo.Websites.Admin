using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class SettingsController : BaseController
    {
        [Inject]
        public ISettingService SettingService { get; set; }

        public virtual ActionResult Edit()
        {
            var currentSetting = SettingService.GetCurrentSetting();

            var model = currentSetting.MapTo<SettingEdit>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(SettingEdit model)
        {
            return Handle(model, SettingService.Edit,
                () => RedirectToAction(MVC.Settings.Edit()),
                RedirectToAction(MVC.Settings.Edit()));
        }
    }
}