using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Models
{
    public class TaskEdit
    {
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public TaskType Type { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Size { get; set; }
        public TaskUrgency Urgency { get; set; }
        public int ProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ForcedPriority { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }

        public string Identifier()
        {
            return Task.TaskIdentifier(Title, Type);
        }
    }
}
