using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL
{
    public class RoleManager : RoleManager<UserRole>
    {
        public RoleManager(Context context)
            : base(new RoleStore<UserRole>(context))
        { }
    }
}
