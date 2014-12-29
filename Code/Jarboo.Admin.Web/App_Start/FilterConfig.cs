using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.Web.Infrastructure;

using StackExchange.Profiling.Mvc;

namespace Jarboo.Admin.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidateAntiForgeryTokenWrapperAttribute(HttpVerbs.Post));
            filters.Add(new ProfilingActionFilter());
        }
    }
}