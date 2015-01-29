using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Authorization;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class Rights
    {
        private static readonly Dictionary<UserRoles, Dictionary<string, HashSet<string>>> rightsByRoles = new Dictionary<UserRoles, Dictionary<string, HashSet<string>>>();
        private static readonly Dictionary<string, HashSet<string>> anonymousRights = new Dictionary<string, HashSet<string>>();
        
        static Rights()
        {
            FillRights();
        }

        private static void FillRights()
        {
            FillAllUserRights(anonymousRights);
            FillAnonymousRights(anonymousRights);

            rightsByRoles[UserRoles.Customer] = new Dictionary<string, HashSet<string>>();
            FillAllUserRights(rightsByRoles[UserRoles.Customer]);
            FillCustomerRights(rightsByRoles[UserRoles.Customer]);
            FillAuthorizedUserRights(rightsByRoles[UserRoles.Customer]);
        }
        private static void FillAllUserRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(MVC.Error.Index());
            rights.Add(MVC.Error.NotFound());
        }
        private static void FillAnonymousRights(Dictionary<string, HashSet<string>> anonymousRights)
        {
            anonymousRights.Add(MVC.Accounts.Login());
            anonymousRights.Add(MVC.Accounts.Register());
        }
        private static void FillAuthorizedUserRights(Dictionary<string, HashSet<string>> authorizedUserRights)
        {
            authorizedUserRights.Add("Elmah", "Index");

            authorizedUserRights.Add(MVC.Error.AccessDenied());

            authorizedUserRights.Add(MVC.Accounts.Edit());
            authorizedUserRights.Add(MVC.Accounts.ChangePassword());
            authorizedUserRights.Add(MVC.Accounts.View());
            authorizedUserRights.Add(MVC.Accounts.Logout());
        }
        private static void FillCustomerRights(Dictionary<string, HashSet<string>> customerRights)
        {
            customerRights.Add(MVC.Home.Index());

            customerRights.Add(MVC.Customers.View());

            customerRights.Add(MVC.Documentations.Index());
            customerRights.Add(MVC.Documentations.View());
            customerRights.Add(MVC.Documentations.Create());
            customerRights.Add(MVC.Documentations.Edit());
            customerRights.Add(MVC.Documentations.Delete());
            customerRights.Add(MVC.Documentations.List());

            customerRights.Add(MVC.Employees.Index());
            customerRights.Add(MVC.Employees.View());

            customerRights.Add(MVC.Projects.View());
            customerRights.Add(MVC.Projects.Create());
            customerRights.Add(MVC.Projects.Edit());

            customerRights.Add(MVC.Tasks.Index());
            customerRights.Add(MVC.Tasks.View());
            customerRights.Add(MVC.Tasks.Steps());
            customerRights.Add(MVC.Tasks.NextStep());
            customerRights.Add(MVC.Tasks.Create());
            customerRights.Add(MVC.Tasks.Delete());
            customerRights.Add(MVC.Tasks.ListConfig());
            customerRights.Add(MVC.Tasks.ListData());
            customerRights.Add(MVC.Tasks.List());
        }

        public static bool Can(this WebViewPage view, ActionResult action)
        {
            var call = action as T4MVC_System_Web_Mvc_ActionResult;
            if (call == null)
            {
                throw new ArgumentException("Action must be T4MVC_System_Web_Mvc_ActionResult");
            }

            return view.User.Can(call.Controller, call.Action);
        }
        public static bool Can(this WebViewPage view, string controller, string action)
        {
            return view.User.Can(controller, action);
        }
        public static bool Can(this Controller ctrl, ActionResult action)
        {
            var call = action as T4MVC_System_Web_Mvc_ActionResult;
            if (call == null)
            {
                throw new ArgumentException("Action must be T4MVC_System_Web_Mvc_ActionResult");
            }

            return ctrl.User.Can(call.Controller, call.Action);
        }
        public static bool Can(this Controller ctrl, string controller, string action)
        {
            return ctrl.User.Can(controller, action);
        }
        public static bool Can(this IPrincipal principal, string controller, string action)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return anonymousRights.Can(controller, action);
            }

            foreach (var userRole in BL.Authorization.Rights.Roles)
            {
                if (principal.IsInRole(userRole.ToString()) && Can(userRole, controller, action))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool Can(UserRoles role, string controller, string action)
        {
            if (role == UserRoles.Admin)
            {
                return true;
            }

            if (!rightsByRoles.ContainsKey(role))
            {
                return false;
            }

            return rightsByRoles[role].Can(controller, action);
        }

        public static void Add(this Dictionary<string, HashSet<string>> dict, ActionResult action)
        {
            var call = action as T4MVC_System_Web_Mvc_ActionResult;
            if (call == null)
            {
                throw new ArgumentException("Action must be T4MVC_System_Web_Mvc_ActionResult");
            }

            if (!dict.ContainsKey(call.Controller))
            {
                dict[call.Controller] = new HashSet<string>();
            }

            dict[call.Controller].Add(call.Action);
        }
    }
}