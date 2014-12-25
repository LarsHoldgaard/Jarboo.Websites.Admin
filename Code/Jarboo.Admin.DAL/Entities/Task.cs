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

        public string FolderLink { get; set; }
        public string CardLink { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual List<TaskStep> Steps { get; set; }

        public string Step()
        {
            if (Done)
            {
                return "Done";
            }

            if (Steps.Count == 0)
            {
                return "Unknown";
            }

            return Steps.Last().Step.ToString();
        }

        public static string TaskFullTitle(string title, TaskType type)
        {
            return LetterForType(type) + "_" + title;
        }
        private static string LetterForType(TaskType type)
        {
            switch (type)
            {
                case TaskType.Bug:
                    return "B";
                case TaskType.Feature:
                    return "F";
                case TaskType.Project:
                    return "P";
                default:
                    throw new Exception("Unnknown task type " + type);
            }
        }
    }
}
