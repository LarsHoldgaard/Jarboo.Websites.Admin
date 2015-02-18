using System.Collections.Generic;

namespace Jarboo.Admin.Web.Models.Time
{
    public class TimeListViewModel 
    {
      
        public IEnumerable<DAL.Entities.SpentTime> Times { get; set; }


    }
}