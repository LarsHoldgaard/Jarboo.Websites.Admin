using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class SpentTimeOnProject
    {
        public SpentTimeOnProject()
        {
            Date = DateTime.Now.Date;
        }

        public int EmployeeId { get; set; }
        [Range(0.5, int.MaxValue, ErrorMessage = "Must be bigger than {1}")]
        public decimal Hours { get; set; }
        public int ProjectId { get; set; }
        public DateTime Date { get; set; }
    }
}
