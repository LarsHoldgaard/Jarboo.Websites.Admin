﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Models
{
    public class TaskCreate
    {
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public TaskType Type { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Size { get; set; }
        public TaskUrgency Urgency { get; set; }
        public int ProjectId { get; set; }
    }
}
