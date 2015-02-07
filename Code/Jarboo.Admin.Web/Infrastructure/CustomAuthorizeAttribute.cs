using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var rd = httpContext.Request.RequestContext.RouteData;
            string action = rd.GetRequiredString("action");
            string controller = rd.GetRequiredString("controller");

            return httpContext.User.Can(controller, action);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(MVC.Error.AccessDenied().GetT4MVCResult().RouteValueDictionary);
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}