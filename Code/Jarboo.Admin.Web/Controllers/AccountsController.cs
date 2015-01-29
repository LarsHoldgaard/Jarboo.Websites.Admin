using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
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
        [Inject]
        public IUserService UserService { get; set; }

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
            return View(new UserCreate());
        }

        [HttpPost]
        public virtual ActionResult Register(UserCreate model, string returnUrl = "")
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

        public virtual ActionResult View(string id = null)
        {
            id = id ?? CurrentUser.Id;
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public virtual ActionResult Edit(string id = null)
        {
            id = id ?? CurrentUser.Id;
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userEdit = user.MapTo<UserEdit>();
            return View(userEdit);
        }

        [HttpPost]
        public virtual ActionResult Edit(UserEdit model)
        {
            return Handle(model, UserService.Edit,
                RedirectToAction(MVC.Accounts.View(model.UserId)),
                RedirectToAction(MVC.Accounts.Edit(model.UserId)));
        }

        public virtual ActionResult ChangePassword(string id = null)
        {
            id = id ?? CurrentUser.Id;
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userEdit = user.MapTo<UserPasswordChange>();
            return View(userEdit);
        }

        [HttpPost]
        public virtual ActionResult ChangePassword(UserPasswordChange model)
        {
            return Handle(model, UserService.ChangePassword,
                RedirectToAction(MVC.Accounts.View(model.UserId)),
                RedirectToAction(MVC.Accounts.ChangePassword(model.UserId)));
        }

        public virtual ActionResult SetPassword(string id = null)
        {
            id = id ?? CurrentUser.Id;
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userEdit = user.MapTo<UserPasswordSet>();
            return View(userEdit);
        }

        [HttpPost]
        public virtual ActionResult SetPassword(UserPasswordSet model)
        {
            return Handle(model, UserService.SetPassword,
                RedirectToAction(MVC.Accounts.View(model.UserId)),
                RedirectToAction(MVC.Accounts.SetPassword(model.UserId)));
        }
    }
}