using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Jarboo.Admin.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/js/validation").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/Site/jquery.validation.fix.js",
                        "~/Scripts/Site/validation.style.js"));

            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                "~/Scripts/bootstrap.js"));



            bundles.Add(new StyleBundle("~/css/site").Include(
                "~/Content/main.css"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                "~/Content/bootstrap.css"));
        }
    }
}