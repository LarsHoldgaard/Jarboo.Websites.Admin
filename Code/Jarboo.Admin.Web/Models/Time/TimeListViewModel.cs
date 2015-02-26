using System.Collections.Generic;

namespace Jarboo.Admin.Web.Models.Time
{
    public class TimeListViewModel
    {
        public IEnumerable<DAL.Entities.SpentTime> Times { get; set; }
        public int? ProjectId { get; set; }
        public int TaskId { get; set; }
        public decimal? TotalHours { get; set; }
        
    }
}