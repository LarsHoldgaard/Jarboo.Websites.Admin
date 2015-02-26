using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    public enum TaskStepEnum
    {
        Specification,
        Architecture,
        Developing,
        [Display(Name = "Code Review")]
        CodeReview,
        Test
    }

    public class TaskStep : BaseEntity
    {
        public TaskStep()
        {
            SpentTimes = new List<SpentTime>();
        }

        [Key]
        [Column(Order = 0)]
        public int TaskId { get; set; }
        [Key]
        [Column(Order = 1)]
        public TaskStepEnum Step { get; set; }
        public DateTime? DateEnd { get; set; }

        public virtual Task Task { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public List<SpentTime> SpentTimes { get; set; }

        public bool Done()
        {
            return DateEnd.HasValue;
        }

        public static TaskStepEnum First()
        {
            return 0;
        }
        public static TaskStepEnum? Next(TaskStepEnum taskStep)
        {
            var nextVal = ((int)taskStep) + 1;
            if (Enum.IsDefined(typeof(TaskStepEnum), nextVal))
            {
                return (TaskStepEnum)nextVal;
            }

            return null;
        }
    }
}
