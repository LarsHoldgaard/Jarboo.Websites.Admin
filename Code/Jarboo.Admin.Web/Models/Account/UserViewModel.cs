using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Account
{
    public class UserViewModel : Jarboo.Admin.DAL.Entities.User
    {
        public string RoleNames { get; set; }

        public string LastLogin
        {
            get
            {
                return DateLastLogin.HasValue ? DateLastLogin.Value.ToString() : "Never";
            }
        }
    }
}