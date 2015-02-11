using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.BL.Models
{
    public class EmployeeCreate
    {
        public EmployeeCreate()
        {
            Positions = new List<Position>();
        }

        public int EmployeeId { get; set; }
        [Required]
        public string FullName { get; set; }
        public string SkypeName { get; set; }
       
        [Required][EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public double HourlyPrice { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public virtual List<Position> Positions { get; set; }
    }
}
