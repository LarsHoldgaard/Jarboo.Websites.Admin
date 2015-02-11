using Jarboo.Admin.BL.Filters;

namespace Jarboo.Admin.Web.Models.Report
{
    public class ReportsListViewModel
    {
        public bool ShowProject { get; set; }
        public ReportFilter ReportFilter { get; set; }
        public ReportSorting Sorting { get; set; }
    }
}