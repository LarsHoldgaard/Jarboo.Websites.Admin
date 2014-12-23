﻿using System;
using System.ComponentModel.DataAnnotations;

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
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public TaskType Type { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Size must be bigger than {1}")]
        public int Size { get; set; }
        public TaskUrgency Urgency { get; set; }

        public string FolderLink { get; set; }
        public string CardLink { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

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
