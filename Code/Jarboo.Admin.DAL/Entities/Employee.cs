using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Employee : BaseEntity
    {
        public Employee()
        {
            Positions = new List<EmployeePosition>();
            TaskSteps = new List<TaskStep>();
            SpentTimes = new List<SpentTime>();
        }

        public int EmployeeId { get; set; }
        [Required]
        public string FullName { get; set; }
        public string SkypeName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal HourlyPrice { get; set; }
        public DateTime? DateDeleted { get; set; }

        public User User { get; set; }

        public virtual List<EmployeePosition> Positions { get; set; }
        public virtual List<TaskStep> TaskSteps { get; set; }
        public virtual List<SpentTime> SpentTimes { get; set; }

        [DisplayName("Is Hired?")]
        public bool IsHired { get; set; }

        public bool Deleted()
        {
            return DateDeleted.HasValue;
        }
    }
}
