using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

namespace Jarboo.Admin.BL.Authorization
{
    public interface IAuth
    {
        User User {get;}

        bool Can(string entities, string action);
    }

    public abstract class AuthBase : IAuth
    {
        public UserManager<User> UserManager { get; set; }

        protected AuthBase(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public abstract string UserName { get; }

        private User _user;
        public User User
        {
            get
            {
                if (_user == null)
                {
                    _user = UserManager.FindByName(UserName);
                }
                return _user;
            }
        }

        public bool Can(string entities, string action)
        {
            return User.Can((userRole) => UserManager.IsInRole(User.Id, userRole.ToString()), entities, action);
        }
    }
}
