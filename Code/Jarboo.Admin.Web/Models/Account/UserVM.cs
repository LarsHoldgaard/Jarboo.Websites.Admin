using Jarboo.Admin.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.Account
{
    public class UserVM : User
    {
        public string RoleNames { get; set; }
    }
}