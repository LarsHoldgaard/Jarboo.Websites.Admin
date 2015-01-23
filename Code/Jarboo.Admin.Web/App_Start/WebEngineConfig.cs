using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.Web.Infrastructure;

using StackExchange.Profiling.Mvc;

namespace Jarboo.Admin.Web.App_Start
{
    public class WebEngineConfig
    {
        public static void RegisterWebEngines()
        {
            ViewEngines.Engines.Clear();
            var razorViewEngine = new RazorViewEngine();

            if (Configuration.Instance.IsDebug())
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(razorViewEngine));
            }
            else
            {
                ViewEngines.Engines.Add(razorViewEngine);
            }
        }
    }
}