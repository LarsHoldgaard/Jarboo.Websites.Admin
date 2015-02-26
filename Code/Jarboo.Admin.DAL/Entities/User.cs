using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL.Entities
{
    public class User : IdentityUser, IBaseEntity
    {
        public User()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        [Index("IX_DisplayName", 1, IsUnique = true)]
        [StringLength(450)]
        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateLastLogin{ get; set; }

        public Customer Customer { get; set; }
        [InverseProperty("UserCustomers")]
        public virtual Customer UserCustomer { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("UserCustomer")]
        public int? UserCustomerId { get; set; }

        public int? CustomerId
        {
            get
            {
                return Customer == null ? null : (int?)Customer.CustomerId;
            }
        }
        
        public int? EmployeeId
        {
            get
            {
                return Employee == null ? null : (int?)Employee.EmployeeId;
            }
        }
    }
}
