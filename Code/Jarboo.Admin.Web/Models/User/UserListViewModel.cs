using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jarboo.Admin.Web.Models.User
{
    public class UserListViewModel
    {
        public IEnumerable<DAL.Entities.User> Users { get; set; }
    }
}