using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL;
using Jarboo.Admin.Web.Infrastructure;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class BaseController : Controller
    {
        protected const string ModelStateKey = "ModelStateKey";
        private const string TempErrorsKey = "TempErrorsKey";
        private const string TempSuccessesKey = "TempSuccessesKey";

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
                handler(model, ModelState.Wrap());

                if (ModelState.IsValid)
                {
                    foreach (var message in messages)
                    {
                        AddSuccess(message);
                    }
                    return successResult();
                }
            }

            TempData[ModelStateKey] = ModelState;
            return failureResult();
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

            base.OnActionExecuted(filterContext);
        }
    }
}