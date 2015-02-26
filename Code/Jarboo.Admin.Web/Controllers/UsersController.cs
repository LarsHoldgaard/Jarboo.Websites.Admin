using Jarboo.Admin.BL;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.Web.Models.User;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class UsersController : Controller
    {
        [Inject]
        public IUserService UserService { get; set; }

        [ChildActionOnly]
        public virtual ActionResult CustomerUsers(int customerId)
        {
            var users = UserService.GetAll(Query.ForUser().Filter(x => x.ByUserCustomerId(customerId)));
            var model = new UserListViewModel() {Users = users};
            return View("_UserList", model);
        }
    }
}