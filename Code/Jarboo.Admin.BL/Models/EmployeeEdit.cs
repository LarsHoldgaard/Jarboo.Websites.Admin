using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Models
{
    public class EmployeeEdit
    {
        public EmployeeEdit()
        {
            Positions = new List<Position>();
        }

        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string SkypeName { get; set; }
        [Required]
        public string TrelloId { get; set; }
        [Required]
        public string Country { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public double HourlyPrice { get; set; }

        public virtual List<Position> Positions { get; set; }
    }
}
