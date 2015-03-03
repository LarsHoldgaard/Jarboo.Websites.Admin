using System.ComponentModel.DataAnnotations;
using Jarboo.Admin.DAL.Entities;
using System;

namespace Jarboo.Admin.BL.Models
{
    public class SpentTimeOnTask
    {
        public SpentTimeOnTask()
        {
            Date = DateTime.Now.Date;
        }

        public int EmployeeId { get; set; }
        [Range(0.5, int.MaxValue, ErrorMessage = "Must be bigger than {1}")]
        public decimal? Hours { get; set; }
        public int TaskId { get; set; }
        public string Roles { get; set; }
        public TaskStep TaskStep { get; set; }
        public DateTime Date { get; set; }
        public decimal? Price { get; set; }
        public TaskStepEnum Step { get; set; }

    }
}
