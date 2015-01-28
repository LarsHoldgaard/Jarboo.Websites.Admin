using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Authorization
{
    public static class Rights
    {
        public const string ViewAll = "ViewAll";
        public const string ViewSpecial = "ViewSpecial";
        public const string AddAny = "AddAny";
        public const string AddSpecial = "AddSpecial";
        public const string EditAny = "EditAny";
        public const string EditSpecial = "EditSpecial";
        public const string DeleteAny = "DeleteAny";
        public const string DeleteSpecial = "DeleteSpecial";

        public class Customers
        {
            public static readonly string Name = typeof(Customers).Name;
        }
        public class Documentations
        {
            public static readonly string Name = typeof(Documentations).Name;
        }
        public class Employees
        {
            public static readonly string Name = typeof(Employees).Name;
        }
        public class Projects
        {
            public static readonly string Name = typeof(Projects).Name;
        }
        public class Tasks
        {
            public static readonly string Name = typeof(Tasks).Name;
        }

        private static readonly Dictionary<UserRoles, Dictionary<string, HashSet<string>>> rightsByRoles = new Dictionary<UserRoles, Dictionary<string, HashSet<string>>>();
        private static readonly Dictionary<string, HashSet<string>> anonymousRights = new Dictionary<string, HashSet<string>>();
        public static List<UserRoles> Roles = Enum.GetValues(typeof(UserRoles)).OfType<UserRoles>().ToList();

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

        }
        private static void FillAnonymousRights(Dictionary<string, HashSet<string>> anonymousRights)
        {

        }
        private static void FillAuthorizedUserRights(Dictionary<string, HashSet<string>> authorizedUserRights)
        {

        }
        private static void FillCustomerRights(Dictionary<string, HashSet<string>> customerRights)
        {
            customerRights.Add(Customers.Name, Rights.ViewSpecial);

            customerRights.Add(Projects.Name, ViewSpecial);
            customerRights.Add(Projects.Name, AddSpecial);
            customerRights.Add(Projects.Name, EditSpecial);
            customerRights.Add(Projects.Name, DeleteSpecial);

            customerRights.Add(Tasks.Name, ViewSpecial);
            customerRights.Add(Tasks.Name, AddSpecial);
            customerRights.Add(Tasks.Name, EditSpecial);
            customerRights.Add(Tasks.Name, DeleteSpecial);

            customerRights.Add(Employees.Name, ViewAll);

            customerRights.Add(Documentations.Name, ViewSpecial);
            customerRights.Add(Documentations.Name, AddSpecial);
            customerRights.Add(Documentations.Name, EditSpecial);
            customerRights.Add(Documentations.Name, DeleteSpecial);
        }

        public static void Add(this Dictionary<string, HashSet<string>> dict, string s1, string s2)
        {
            if (!dict.ContainsKey(s1))
            {
                dict[s1] = new HashSet<string>();
            }

            dict[s1].Add(s2);
        }

        public static bool Can(this User user, Func<UserRoles, bool> isInRole,  string entities, string action)
        {
            if (user == null)
            {
                return anonymousRights.Can(entities, action);
            }

            foreach (var userRole in Rights.Roles)
            {
                if (isInRole(userRole) && Can(userRole, entities, action))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool Can(UserRoles role, string entities, string action)
        {
            if (role == UserRoles.Admin)
            {
                return true;
            }

            if (!rightsByRoles.ContainsKey(role))
            {
                return false;
            }

            return rightsByRoles[role].Can(entities, action);
        }
        public static bool Can(this Dictionary<string, HashSet<string>> rights, string entities, string action)
        {
            if (!rights.ContainsKey(entities))
            {
                return false;
            }

            if (rights[entities].Contains(action))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
