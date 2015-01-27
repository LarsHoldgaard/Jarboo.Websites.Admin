using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Infrastructure
{
    public static class Rights
    {
        private static readonly Dictionary<UserRoles, Dictionary<string, HashSet<string>>> rights = new Dictionary<UserRoles, Dictionary<string, HashSet<string>>>();
        public static List<UserRoles> Roles = Enum.GetValues(typeof(UserRoles)).OfType<UserRoles>().ToList();

        static Rights()
        {
            FillRights();
        }

        private static void FillRights()
        {
            var customerRights = new Dictionary<string, HashSet<string>>();

            customerRights.Add(MVC.Home.Index());

            rights[UserRoles.Customer] = customerRights;
        }
        public static void Add(this Dictionary<string, HashSet<string>> dict, string s1, string s2)
        {
            if (!dict.ContainsKey(s1))
            {
                dict[s1] = new HashSet<string>();
            }

            dict[s1].Add(s2);
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

        public static bool Can(UserRoles role, string controller, string action)
        {
            if (role == UserRoles.Admin)
            {
                return true;
            }

            if (!rights.ContainsKey(role))
            {
                return false;
            }

            var roleRights = rights[role];
            if (!roleRights.ContainsKey(controller))
            {
                return false;
            }

            return roleRights[controller].Contains(action);
        }
    }
}