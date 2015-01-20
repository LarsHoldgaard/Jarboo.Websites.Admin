using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class Helper
    {
        public static RequestContext GetRequestContext()
        {
            var context = new HttpContextWrapper(System.Web.HttpContext.Current);
            var routeData = RouteTable.Routes.GetRouteData(context);

            return new RequestContext(context, routeData);
        }
    }
}