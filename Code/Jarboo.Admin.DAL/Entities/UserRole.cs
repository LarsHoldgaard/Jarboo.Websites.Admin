using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL.Entities
{
    public enum UserRoles
    {
        Admin,
        Customer,
        Employee
    }

    public class UserRole : IdentityRole
    {
    }
}
