using System.Linq;

namespace Jarboo.Admin.Web.Models.Report
{
    public class ReportViewModel : DAL.Entities.Task
    {
        public string Step()
        {
            if (Done)
            {
                return "Done";
            }

            if (Steps.Count == 0)
            {
                return "Unknown";
            }

            return Steps.Last().Step.ToString();
        }
    }
}