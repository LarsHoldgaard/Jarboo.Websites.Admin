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
using Jarboo.Admin.Web.Infrastructure;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Ninject;
using Jarboo.Admin.BL.Services.Interfaces;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AccountsController : BaseController
    {
        [Inject]
        public IAccountService AccountService { get; set; }
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public RoleManager<UserRole> RoleManager { get; set; }

        public virtual ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public virtual ActionResult Login(LoginViewModel model, string returnUrl = "")
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

            return this.RedirectToLocalUrl(returnUrl, MVC.Home.Index());
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
                () => this.Login(new LoginViewModel()
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

            if (user.Customer == null)
            {
                var userEdit = user.MapTo<UserEdit>();
                return View(userEdit);
            }
            else
            {
                var userEdit = user.MapTo<UserCustomerEdit>();
                return View(MVC.Accounts.Views.CustomerEdit, userEdit);
            }
        }

        [HttpPost]
        public virtual ActionResult Edit(UserEdit model)
        {
            return Handle(model, UserService.Edit,
                () => this.OnEditSuccess(model),
                RedirectToAction(MVC.Accounts.Edit(model.UserId)));
        }
        [HttpPost]
        public virtual ActionResult CustomerEdit(UserCustomerEdit model)
        {
            return Handle(model, UserService.EditCustomer,
                () => this.OnEditSuccess(model),
                RedirectToAction(MVC.Accounts.Edit(model.UserId)));
        }
        private ActionResult OnEditSuccess(UserEdit model)
        {
            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
            {
                return RedirectToAction(MVC.Accounts.Login());
            }

            var identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = true,
            }, identity);

            return RedirectToAction(MVC.Accounts.View(model.UserId));
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

        public virtual ActionResult Index()
        {
            var users = UserService.GetAll(Query.ForUser().Include(x => x.Customer().Employee().Roles())).Decorate<User, UserViewModel>();
            var roles = RoleManager.Roles.ToList();
            foreach (var user in users)
            {
                user.RoleNames = string.Join(" ",
                    roles.Where(x => user.Roles.Any(y => y.RoleId == x.Id)).Select(x => x.Name));
            }

            return View(users);
        }

        public virtual ActionResult RecoverPassword()
        {
            return base.View(new PasswordRecoverViewModel());
        }

        [HttpPost]
        public virtual ActionResult RecoverPassword(PasswordRecoverViewModel model)
        {
            var recoverUlrTemplate = string.Format("{0}?userId={{0}}&code={{1}}", this.Url.ActionAbsolute(MVC.Accounts.ResetPassword()));

            var blModel = new PasswordRecover()
                              {
                                  Email = model.Email,
                                  LinkTemplate = recoverUlrTemplate
                              };

            return Handle(blModel, AccountService.RecoverPassword,
                RedirectToAction(MVC.Accounts.Login()),
                RedirectToAction(MVC.Accounts.RecoverPassword()),
                "Check your email for recovery password link");
        }

        public virtual ActionResult ResetPassword(string userId, string code)
        {
            return View(new ResetPassword()
                            {
                                UserId = userId,
                                Code = code
                            });
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public virtual ActionResult ResetPasswordPost(ResetPassword model)
        {
            return Handle(model, AccountService.ResetPassword,
                RedirectToAction(MVC.Accounts.Login()),
                RedirectToAction(MVC.Accounts.ResetPassword(model.UserId, model.Code)),
                "Password changed");

            return View(model);
        }
    }
}