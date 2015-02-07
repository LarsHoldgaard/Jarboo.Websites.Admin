using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public class SpentTime : BaseEntity
    {
        public int SpentTimeId { get; set; }
        [Range(0.5, int.MaxValue, ErrorMessage = "Must be bigger than {1}")]
        public decimal Hours { get; set; }
        public double HourlyPrice { get; set; }
        public DateTime? DateVerified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? Accepted { get; set; }

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [ForeignKey("TaskStep"), Column(Order = 0)]
        public int? TaskId { get; set; }
        [ForeignKey("TaskStep"), Column(Order = 1)]
        public TaskStepEnum? Step { get; set; }
        public virtual TaskStep TaskStep { get; set; }

        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int GetProjectId()
        {
            if (ProjectId.HasValue)
            {
                return ProjectId.Value;
            }

            if (TaskStep != null && TaskStep.Task != null)
            {
                return TaskStep.Task.ProjectId;
            }

            return 0;
        }
        public Project GetProject()
        {
            if (Project != null)
            {
                return Project;
            }

            if (TaskStep != null && TaskStep.Task != null && TaskStep.Task.Project != null)
            {
                return TaskStep.Task.Project;
            }

            return null;
        }

        public int GetCustomerId()
        {
            if (Project != null)
            {
                return Project.CustomerId;
            }

            if (TaskStep != null && TaskStep.Task != null && TaskStep.Task.Project != null)
            {
                return TaskStep.Task.Project.CustomerId;
            }

            return 0;
        }
        public Customer GetCustomer()
        {
            if (Project != null && Project.Customer != null)
            {
                return Project.Customer;
            }

            if (TaskStep != null && TaskStep.Task != null && TaskStep.Task.Project != null && TaskStep.Task.Project.Customer != null)
            {
                return TaskStep.Task.Project.Customer;
            }

            return null;
        }
    }
}
