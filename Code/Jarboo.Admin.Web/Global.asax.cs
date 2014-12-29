using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Jarboo.Admin.Web.App_Start;

using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;

namespace Jarboo.Admin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                this.ApplicationStartInternal();
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                throw;
            }

        }
        private void ApplicationStartInternal()
        {
            Elmah.Mvc.Bootstrap.Initialize();
            WebEngineConfig.RegisterWebEngines();
            MiniProfilerEF6.Initialize();
            DatabaseConfig.ConfigureDatabase();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappers();
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}
