using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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
       [Required(ErrorMessage = "Project required")]
        public int ProjectId { get; set; }
        [DisplayName("Employee")]
        public int? EmployeeId { get; set; }
        public int? ForcedPriority { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Description { get; set; }
        public DateTime? DateApproved { get; set; }
       
        public List<SelectListItem> Projects { get; set; }

        [DisplayName("Project")]
        [Editable(false)]
        public string ProjectName { get; set; }


        public string Identifier()
        {
            return Task.TaskIdentifier(Title, Type);
        }
    }
}
