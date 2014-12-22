using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarboo.Admin.DAL.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Customer : BaseEntity
    {
        public Customer()
        {
            Projects = new List<Project>();
        }

        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual List<Project> Projects { get; set; }
    }
}
