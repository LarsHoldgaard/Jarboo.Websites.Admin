using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Models;
using Jarboo.Admin.BL.Services;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Models.Account;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AccountsController : BaseController
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        public virtual IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public virtual ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        public virtual ActionResult Login(LoginVM model, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                PreserveModelState();
                return RedirectToAction(MVC.Accounts.Login(returnUrl));
            }

            var user = UserManager.Find(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                PreserveModelState();
                return RedirectToAction(MVC.Accounts.Login(returnUrl));
            }

            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties()
                                             {
                                                 IsPersistent = model.Persist,
                                             }, identity);

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction(MVC.Home.Index());
            }
            else
            {
                return this.Redirect(returnUrl);
            }
        }
        
        public virtual ActionResult Register(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new Register());
        }

        [HttpPost]
        public virtual ActionResult Register(Register model, string returnUrl = "")
        {
            return Handle(model, AccountService.Register,
                () => this.Login(new LoginVM()
                                {
                                    Email = model.Email,
                                    Password = model.Password,
                                    Persist = true,
                                }, returnUrl),
                RedirectToAction(MVC.Accounts.Register(returnUrl)));
        }

        [HttpPost]
        public virtual ActionResult Logout()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction(MVC.Home.Index());
        }
    }
}