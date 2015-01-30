using System;
using System.Collections.Generic;
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

        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
    }
}
