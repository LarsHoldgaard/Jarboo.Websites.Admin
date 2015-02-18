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
        public const string DisableAny = "DisableAny";
        public const string DisableSpecial = "DisableSpecial";

        public class Accounts
        {
            public static readonly string Name = typeof(Accounts).Name;
        }
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

            public const string NextStepAny = "NextStepAny";
            public const string NextStepSpecial = "NextStepSpecial";
        }
        public class Users
        {
            public static readonly string Name = typeof(Users).Name;

            public const string SetPasswordAny = "SetPasswordAny";
            //public const string SetPasswordSpecial = "SetPasswordSpecial";
        }
        public class SpentTime
        {
            public static readonly string Name = typeof(SpentTime).Name;

            public const string AcceptAny = "AcceptAny";
            public const string DenyAny = "DenyAny";
        }
        public class Question
        {
            public static readonly string Name = typeof(Question).Name;
        }
        public class Comment
        {
            public static readonly string Name = typeof(Comment).Name;
        }
        public class Answer
        {
            public static readonly string Name = typeof(Answer).Name;
        }
        public class Quizzes
        {
            public static readonly string Name = typeof(Quizzes).Name;
        }
        private static readonly Dictionary<UserRoles, Dictionary<string, HashSet<string>>> rightsByRoles = new Dictionary<UserRoles, Dictionary<string, HashSet<string>>>();
        private static readonly Dictionary<string, HashSet<string>> authorizedUserRights = new Dictionary<string, HashSet<string>>();
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

            FillAllUserRights(authorizedUserRights);
            FillAuthorizedUserRights(authorizedUserRights);

            rightsByRoles[UserRoles.Customer] = new Dictionary<string, HashSet<string>>();
            FillCustomerRights(rightsByRoles[UserRoles.Customer]);

            rightsByRoles[UserRoles.Employee] = new Dictionary<string, HashSet<string>>();
            FillEmployeeRights(rightsByRoles[UserRoles.Employee]);
        }
        private static void FillAllUserRights(Dictionary<string, HashSet<string>> rights)
        {

        }
        private static void FillAnonymousRights(Dictionary<string, HashSet<string>> rights)
        {
        }
        private static void FillAuthorizedUserRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(Users.Name, ViewSpecial);
            rights.Add(Users.Name, EditSpecial);
        }
        private static void FillCustomerRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(Customers.Name, ViewSpecial);

            rights.Add(Projects.Name, ViewSpecial);
            rights.Add(Projects.Name, AddSpecial);
            rights.Add(Projects.Name, EditSpecial);

            rights.Add(Tasks.Name, ViewSpecial);
            rights.Add(Tasks.Name, AddSpecial);
            rights.Add(Tasks.Name, EditSpecial);
            rights.Add(Tasks.Name, Tasks.NextStepSpecial);
            rights.Add(Tasks.Name, DisableSpecial);

            rights.Add(Employees.Name, ViewAll);

            rights.Add(Documentations.Name, ViewSpecial);
            rights.Add(Documentations.Name, AddSpecial);
            rights.Add(Documentations.Name, EditSpecial);
            rights.Add(Documentations.Name, DeleteSpecial);

            rights.Add(SpentTime.Name, ViewSpecial);
        }
        private static void FillEmployeeRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(Customers.Name, ViewAll);

            rights.Add(Projects.Name, ViewAll);

            rights.Add(Tasks.Name, ViewSpecial);
            rights.Add(Tasks.Name, Tasks.NextStepSpecial);

            rights.Add(Employees.Name, ViewAll);

            rights.Add(Documentations.Name, ViewSpecial);

            rights.Add(Quizzes.Name, ViewSpecial);

            rights.Add(SpentTime.Name, ViewSpecial);
            rights.Add(SpentTime.Name, AddSpecial);
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

            if (authorizedUserRights.Can(entities, action))
            {
                return true;
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
