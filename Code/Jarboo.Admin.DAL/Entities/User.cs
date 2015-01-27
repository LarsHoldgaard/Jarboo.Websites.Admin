using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public Customer Customer { get; set; }
    }
}
