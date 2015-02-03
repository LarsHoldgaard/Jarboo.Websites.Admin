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
                                        RequireUppercase = false,
                                        RequiredLength = 8,
                                    };
        }
    }
}
