using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Project : BaseEntity
    {
        public Project()
        {
            Documentations = new List<Documentation>();
            Tasks = new List<Task>();
        }

        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<Documentation> Documentations { get; set; }
        public virtual List<Task> Tasks { get; set; }
    }
}
