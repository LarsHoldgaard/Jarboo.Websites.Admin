using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Other;
using Jarboo.Admin.DAL.Entities;
using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Infrastructure.BLExternals;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class BaseController : Controller
    {
        [Inject]
        public UserManager<User> UserManager { get; set; }

        public virtual IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected const string ModelStateKey = "ModelStateKey";
        protected const string TempErrorsKey = "TempErrorsKey";
        protected const string TempSuccessesKey = "TempSuccessesKey";

        protected ActionResult Handle<TModel>(TModel model, Action<TModel, IBusinessErrorCollection> handler, ActionResult successResult, ActionResult failureResult, params string[] messages)
        {
            return Handle(model, handler, () => successResult, () => failureResult, messages);
        }
        protected ActionResult Handle<TModel>(TModel model, Action<TModel, IBusinessErrorCollection> handler, ActionResult successResult, Func<ActionResult> failureResult, params string[] messages)
        {
            return Handle(model, handler, () => successResult, failureResult, messages);
        }
        protected ActionResult Handle<TModel>(TModel model, Action<TModel, IBusinessErrorCollection> handler, Func<ActionResult> successResult, ActionResult failureResult, params string[] messages)
        {
            return Handle(model, handler, successResult, () => failureResult, messages);
        }
        protected ActionResult Handle<TModel>(TModel model, Action<TModel, IBusinessErrorCollection> handler, Func<ActionResult> successResult, Func<ActionResult> failureResult, params string[] messages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    handler(model, ModelState.Wrap());
                }
                catch (ApplicationException ex)
                {
                    Elmah.ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Elmah.Error(ex));
                    AddError(ex.Message);
                    return RedirectToAction(MVC.Error.Index());
                }

                if (ModelState.IsValid)
                {
                    foreach (var message in messages)
                    {
                        AddSuccess(message);
                    }
                    return successResult();
                }
            }

            this.PreserveModelState();
            return failureResult();
        }

        protected void PreserveModelState()
        {
            TempData[ModelStateKey] = ModelState;
        }

        protected void AddSuccess(string text)
        {
            var successes = TempData[TempSuccessesKey] as List<string> ?? new List<string>();
            successes.Add(text);
            TempData[TempSuccessesKey] = successes;
        }
        protected void AddError(string text)
        {
            var errors = TempData[TempErrorsKey] as List<string> ?? new List<string>();
            errors.Add(text);
            TempData[TempErrorsKey] = errors;
        }
        private static void AddError(dynamic ViewBag, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (ViewBag.Errors == null)
            {
                ViewBag.Errors = new List<string>();
            }

            ViewBag.Errors.Add(text);
        }
        private static void AddSuccess(dynamic ViewBag, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (ViewBag.Sucesses == null)
            {
                ViewBag.Sucesses = new List<string>();
            }

            ViewBag.Sucesses.Add(text);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SetCurrentUser() && HttpContext.User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut();
                filterContext.Result = new RedirectResult(HttpContext.Request.Url.ToString());
            }

            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var errors = TempData[TempErrorsKey] as List<string> ?? new List<string>();
            foreach (var error in errors)
            {
                AddError(ViewBag, error);
            }

            var successes = TempData[TempSuccessesKey] as List<string> ?? new List<string>();
            foreach (var success in successes)
            {
                AddSuccess(ViewBag, success);
            }

            if (TempData[ModelStateKey] != null && ModelState.Equals(TempData[ModelStateKey]) == false)
            {
                ModelState.Merge((ModelStateDictionary)TempData[ModelStateKey]);
            }

            if (filterContext.Result is ViewResultBase)
            {
                ViewBag.CurrentUser = CurrentUser;
            }

            base.OnActionExecuted(filterContext);
        }

        protected new ActionResult HttpNotFound()
        {
            //throw new HttpException(404, "Not found");
            return RedirectToAction(MVC.Error.NotFound());
        }
        protected new ActionResult HttpAccessDenied()
        {
            //throw new HttpException(404, "Not found");
            return RedirectToAction(MVC.Error.AccessDenied());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                return;

            if (Configuration.Instance.ErrorMode == CustomErrorsMode.Off)
            {
                return;
            }
            else if ((Configuration.Instance.ErrorMode == CustomErrorsMode.RemoteOnly) && HttpContext.Request.IsLocal)
            {
                return;
            }

            if (filterContext.Exception is ApplicationException)
            {
                AddError(filterContext.Exception.Message);
            }

            filterContext.ExceptionHandled = true;

            filterContext.Result = RedirectToAction(MVC.Error.Index());
        }

        private bool SetCurrentUser()
        {
            CurrentUser = UserManager.FindByName(HttpContext.User.Identity.Name);
            if (CurrentUser != null)
            {
                return true;
            }

            CurrentUser = new User()
                {
                    DisplayName = "Anonymous"
                };

            return false;
        }
        public User CurrentUser { get; set; }
        protected int? UserEmployeeId
        {
            get
            {
                return CurrentUser.Employee == null ? null : (int?)CurrentUser.Employee.EmployeeId;
            }
        }
        protected int? UserCustomerId
        {
            get
            {
                return CurrentUser.Customer == null ? null : (int?)CurrentUser.Customer.CustomerId;
            }
        }

        protected ActionResult RedirectToLocalUrl(string localUrl, ActionResult def)
        {
            if (string.IsNullOrEmpty(localUrl) || !Url.IsLocalUrl(localUrl))
            {
                return RedirectToAction(def);
            }
            else
            {
                return this.Redirect(localUrl);
            }
        }
    }
}