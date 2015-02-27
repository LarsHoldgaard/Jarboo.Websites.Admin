using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jarboo.Admin.DAL.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            Projects = new List<Project>();
            UserCustomers = new List<User>();
        }

        public int CustomerId { get; set; }
        [Index("IX_Name", 1, IsUnique = true)]
        [StringLength(450)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Creator { get; set; }

        public User User { get; set; }

        public virtual List<Project> Projects { get; set; }
        public virtual List<User> UserCustomers { get; set; }
    }
}
