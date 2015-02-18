using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Models
{
    public class SpentTimeOnTask
    {
        public int EmployeeId { get; set; }
        [Range(0.5, int.MaxValue, ErrorMessage = "Must be bigger than {1}")]
        public decimal Hours { get; set; }       
        public int TaskId { get; set; }
        public TaskStep Step { get; set; }
    }
}
