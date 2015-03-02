using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Models
{
    public class TaskNextStep
    {
        public int TaskId { get; set; }
        public int? EmployeeId { get; set; }
        public TaskStepEnum? Step { get; set; }
    }
}
