using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Documentations = new List<Documentation>();
            Tasks = new List<Task>();
            SpentTimes = new List<SpentTime>();
        }

        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BoardName { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<Documentation> Documentations { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<SpentTime> SpentTimes { get; set; }
    }
}
