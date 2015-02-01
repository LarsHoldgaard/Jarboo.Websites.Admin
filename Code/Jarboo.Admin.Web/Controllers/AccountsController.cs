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

        public virtual ActionResult Index()
        {
            var users = UserService.GetAll(Query.ForUser().Include(x => x.Customer().Employee().Roles())).Decorate<User, UserVM>();
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
            return base.View(new PasswordRecover());
        }

        [HttpPost]
        public virtual ActionResult RecoverPassword(PasswordRecover model)
        {
            if (!ModelState.IsValid)
            {
                PreserveModelState();
                return RedirectToAction(MVC.Accounts.RecoverPassword());
            }

            var user = UserManager.FindByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email");
                PreserveModelState();
                return RedirectToAction(MVC.Accounts.RecoverPassword());
            }

            var code = UserManager.GeneratePasswordResetToken(user.Id);

            var recoverUrl = Url.Action(MVC.Accounts.ResetPassword(new BL.Models.ResetPassword()
                {
                    UserId = user.Id,
                    Code = code
                }));

            UserManager.SendEmail(user.Id, "Reset Password",
                "Please reset your password by clicking here: <a href=\"" + recoverUrl + "\">link</a>");

            AddSuccess("Check your email for recovery password link.");
            return RedirectToAction(MVC.Accounts.Login());
        }

        public virtual ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult ResetPassword(ResetPassword model)
        {
            return View();
        }
    }
}