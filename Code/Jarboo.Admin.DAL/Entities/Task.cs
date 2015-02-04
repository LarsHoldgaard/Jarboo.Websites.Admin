using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Jarboo.Admin.DAL.Entities
{
    public enum TaskType
    {
        Feature,
        Bug,
        Project
    }

    public enum TaskUrgency
    {
        VLow,
        Low,
        Medium,
        High,
        VHigh
    }

    public class Task : BaseEntity
    {
        public Task()
        {
            Steps = new List<TaskStep>();
        }

        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public TaskType Type { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Size must be bigger than {1}")]
        public int Size { get; set; }
        public TaskUrgency Urgency { get; set; }
        public bool Done { get; set; }
        public int? ForcedPriority { get; set; }

        public string FolderLink { get; set; }
        public string CardLink { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual List<TaskStep> Steps { get; set; }

        public string Identifier()
        {
            return TaskIdentifier(Title, Type);
        }
        public static string TaskIdentifier(string title, TaskType type)
        {
            return type.GetLetter() + "_" + title;
        }

        public bool Deleted
        {
            get
            {
                return DateDeleted.HasValue;
            }
        }

        public decimal Priority
        {
            get
            {
                var val = 0.0m;
                if (ForcedPriority.HasValue)
                {
                    return ForcedPriority.Value;
                }

                val += (int)this.Urgency * 5;
                val -= this.Size * 0.5m;
                return val;
            }
        }

        public decimal Hours()
        {
            return Steps.SelectMany(x => x.SpentTimes).Aggregate(0m, (a, x) => a + x.Hours);
        }
    }

    public class TaskEqualityComparer : IEqualityComparer<Task>
    {
        public bool Equals(Task x, Task y)
        {
            return x.TaskId == y.TaskId;
        }

        public int GetHashCode(Task obj)
        {
            return obj.TaskId;
        }
    }
}
