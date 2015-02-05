using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jarboo.Admin.DAL
{
    public class UserStore : UserStore<User, UserRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, IUserStore<User>
    {
        public UserStore(Context context)
            : base(context)
        {
            
        }

        protected override Task<User> GetUserAggregateAsync(Expression<Func<User, bool>> filter)
        {
            return
                Users.Include(u => u.Customer)
                    .Include(u => u.Employee)
                    .Include(u => u.Roles)
                    .Include(u => u.Claims)
                    .Include(u => u.Logins)
                    .Include(u => u.Customer)
                    .FirstOrDefaultAsync(filter);
        }
    }
}
