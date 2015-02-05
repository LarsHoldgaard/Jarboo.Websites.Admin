using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jarboo.Admin.DAL.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            Projects = new List<Project>();
        }

        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Creator { get; set; }

        public User User { get; set; }

        public virtual List<Project> Projects { get; set; }
    }
}
