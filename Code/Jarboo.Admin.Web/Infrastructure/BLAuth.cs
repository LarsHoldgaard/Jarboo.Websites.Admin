using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.DAL.Entities;

using Microsoft.AspNet.Identity;

using Ninject;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class BLAuth : AuthBase
    {
        public BLAuth(UserManager<User> userManager)
            : base(userManager)
        { }

        public override string UserName
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
    }
}