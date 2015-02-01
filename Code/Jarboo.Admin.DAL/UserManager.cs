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
    public class UserManager : UserManager<User>
    {
        public UserManager(Context context)
            : this(context, null)
        {
        }
        public UserManager(Context context, IUserTokenProvider<User, string> tokenProvider)
            : base(new UserStore(context))
        {
            UserTokenProvider = tokenProvider;

            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator()
                                    {
                                        RequireDigit = true,
                                        RequireLowercase = true,
                                        RequireNonLetterOrDigit = true,
                                        RequireUppercase = true,
                                        RequiredLength = 8,
                                    };
        }

        public override Task<User> FindByIdAsync(string userId)
        {
            return base.FindByIdAsync(userId);
        }

        public override Task<User> FindByEmailAsync(string email)
        {
            return base.FindByEmailAsync(email);
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return base.FindByNameAsync(userName);
        }

        public override Task<User> FindAsync(string userName, string password)
        {
            return base.FindAsync(userName, password);
        }

        public override Task<User> FindAsync(UserLoginInfo login)
        {
            return base.FindAsync(login);
        }

        public override Task<bool> IsInRoleAsync(string userId, string role)
        {
            return base.IsInRoleAsync(userId, role);
        }
    }
}
