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
            bundles.Add(new StyleBundle("~/css/site").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/invoice.min.css",
                "~/Content/css/lockscreen.min.css",
                "~/Content/css/smartadmin-production-plugins.min.css",
                "~/Content/css/smartadmin-production.min.css",
                "~/Content/css/smartadmin-rtl.backup.min.css",
                "~/Content/css/smartadmin-rtl.min.css",
                "~/Content/css/smartadmin-skins.min.css",
                "~/Content/css/your_style.css",
                "~/Content/main.css",
                "~/Content/datepicker3.css"
            ));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                // SmartAdmin
                "~/scripts/app.config.js",
                "~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                "~/scripts/bootstrap/bootstrap.min.js",
                "~/scripts/notification/SmartNotification.min.js",
                "~/scripts/smartwidgets/jarvis.widget.min.js",
                "~/scripts/plugin/jquery-validate/jquery.validate.min.js",
                "~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
                "~/scripts/plugin/select2/select2.min.js",
                "~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                "~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                "~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                "~/scripts/plugin/fastclick/fastclick.min.js",
                "~/scripts/app.min.js",

                // Voice command : plugin
                "~/scripts/speech/voicecommand.min.js",

                //  SmartChat UI : plugin
                "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.min.js",

                // DataTables : plugin
                "~/scripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js",

                // jQGrid : plugin
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-en.min.js",

                // Forms : plugin
                "~/scripts/plugin/jquery-form/jquery-form.min.js",

                // Flot Chart Plugin: Flot Engine, Flot Resizer, Flot Tooltip, Morris, Sparkline, EasyPie
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js",

                // Vector Maps Plugin: Vectormap engine, Vectormap language
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js",

                // Full Calendar
                "~/scripts/plugin/moment/moment.min.js",
                "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js",

                // Bottom
                "~/scripts/bootstrap-datepicker.js",
                "~/scripts/jquery.validate.min.js",
                "~/scripts/jquery.validate.unobtrusive.min.js",
                "~/scripts/Site/smartAdmin.dataTable.js",
                "~/scripts/jquery.bonnet.ajax-dropdownlist.js",
                "~/scripts/Site/site.js"
            ));

            /*
            bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
                "~/scripts/plugin/moment/moment.min.js",
                "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                "~/scripts/plugin/sparkline/jquery.sparkline.min.js",
                "~/scripts/plugin/morris/morris.min.js",
                "~/scripts/plugin/morris/raphael.min.js",
                "~/scripts/plugin/flot/jquery.flot.cust.min.js",
                "~/scripts/plugin/flot/jquery.flot.resize.min.js",
                "~/scripts/plugin/flot/jquery.flot.time.min.js",
                "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
                "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
                "~/scripts/plugin/flot/jquery.flot.pie.min.js",
                "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
                "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
                "~/scripts/plugin/chartjs/chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
                "~/scripts/plugin/datatables/jquery.dataTables.min.js",
                "~/scripts/plugin/datatables/dataTables.colVis.min.js",
                "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
                "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
                "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
                "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
                "~/scripts/plugin/jqgrid/grid.locale-en.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/forms").Include(
                "~/scripts/plugin/jquery-form/jquery-form.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
                "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
                "~/scripts/smart-chat-ui/smart.chat.manager.min.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
                "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
                "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
                ));
             
            bundles.Add(new ScriptBundle("~/js/site").Include(
                "~/Scripts/Site/smartAdmin.dataTable.js",
                "~/Scripts/jquery.bonnet.ajax-dropdownlist.js",
                "~/Scripts/Site/site.js"));

            bundles.Add(new ScriptBundle("~/js/validation").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/Site/validation.style.js"));

            bundles.Add(new ScriptBundle("~/js/bootstrap-datepicker").Include(
                "~/Scripts/bootstrap-datepicker.js"));
            */
            BundleTable.EnableOptimizations = true;
        }
    }
}