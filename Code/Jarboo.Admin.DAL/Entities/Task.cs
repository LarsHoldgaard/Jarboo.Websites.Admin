using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;

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
        public int TaskID { get; set; }
        [Required]
        public string Title { get; set; }
        public TaskType Type { get; set; }
        public int Size { get; set; }
        public TaskUrgency Urgency { get; set; }

        public string FolderLink { get; set; }
        public string CardLink { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
