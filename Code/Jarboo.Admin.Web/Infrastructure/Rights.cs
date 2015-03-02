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
        private static readonly Dictionary<string, HashSet<string>> authorizedUserRights = new Dictionary<string, HashSet<string>>();
        private static readonly Dictionary<string, HashSet<string>> anonymousRights = new Dictionary<string, HashSet<string>>();

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
            rights.Add(MVC.Error.Index());
            rights.Add(MVC.Error.NotFound());
            authorizedUserRights.Add(MVC.Error.AccessDenied());
        }
        private static void FillAnonymousRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(MVC.Accounts.Login());
            rights.Add(MVC.Accounts.Register());
            rights.Add(MVC.Accounts.RecoverPassword());
            rights.Add(MVC.Accounts.ResetPassword());

            rights.Add(MVC.Home.Index());
            rights.Add(MVC.Home.Landing());
        }
        private static void FillAuthorizedUserRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add("Elmah", "Index");

            rights.Add(MVC.Home.Index());

            rights.Add(MVC.Accounts.Edit());
            rights.Add(MVC.Accounts.ChangePassword());
            rights.Add(MVC.Accounts.View());
            rights.Add(MVC.Accounts.Logout());
        }
        private static void FillCustomerRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(MVC.Accounts.CustomerEdit());

            rights.Add(MVC.Home.Dashboard());

            rights.Add(MVC.Customers.View());

            rights.Add(MVC.Documentations.View());
            rights.Add(MVC.Documentations.Create());
            rights.Add(MVC.Documentations.Edit());
            rights.Add(MVC.Documentations.Delete());
            rights.Add(MVC.Documentations.List());

            rights.Add(MVC.Employees.View());

            rights.Add(MVC.Projects.View());
            rights.Add(MVC.Projects.Create());
            rights.Add(MVC.Projects.Edit());

            rights.Add(MVC.Tasks.Index());
            rights.Add(MVC.Tasks.View());
            rights.Add(MVC.Tasks.Steps());
            rights.Add(MVC.Tasks.NextStep());
            rights.Add(MVC.Tasks.Create());
            rights.Add(MVC.Tasks.Delete());
            rights.Add(MVC.Tasks.ListConfig());
            rights.Add(MVC.Tasks.ListData());
            rights.Add(MVC.Tasks.List());
            rights.Add(MVC.Tasks.TasksPerDayChartData());

            rights.Add(MVC.Tasks.PendingTask());
            rights.Add(MVC.Tasks.PendingTaskView());
            rights.Add(MVC.Tasks.ApproveTask());
            rights.Add(MVC.Tasks.PendingTaskList());


            rights.Add(MVC.Reporting.Index());
            rights.Add(MVC.Reporting.List());
            rights.Add(MVC.Reporting.ListConfig());
            rights.Add(MVC.Reporting.ListData());

            rights.Add(MVC.Questions.QuestionList());
            rights.Add(MVC.Questions.Create());

            rights.Add(MVC.Answer.Index());
            rights.Add(MVC.Answer.AnswerList());
            rights.Add(MVC.Answer.Create());
        }
        private static void FillEmployeeRights(Dictionary<string, HashSet<string>> rights)
        {
            rights.Add(MVC.Customers.Index());
            rights.Add(MVC.Customers.View());

            rights.Add(MVC.Documentations.View());
            rights.Add(MVC.Documentations.List());

            rights.Add(MVC.Quizzes.View());
            rights.Add(MVC.Quizzes.List());

            rights.Add(MVC.Employees.Index());
            rights.Add(MVC.Employees.View());

            rights.Add(MVC.Projects.Index());
            rights.Add(MVC.Projects.View());
            rights.Add(MVC.Projects.AddHours());

            rights.Add(MVC.Tasks.View());
            rights.Add(MVC.Tasks.Steps());
            rights.Add(MVC.Tasks.NextStep());
            rights.Add(MVC.Tasks.ListConfig());
            rights.Add(MVC.Tasks.ListData());
            rights.Add(MVC.Tasks.List());
            rights.Add(MVC.Tasks.NextTask());
            rights.Add(MVC.Tasks.AddHours());

            rights.Add(MVC.SpentTime.Index());

            rights.Add(MVC.Answer.Index());
            rights.Add(MVC.Answer.AnswerList());
            rights.Add(MVC.Answer.Create());

            rights.Add(MVC.Comments.CommentList());
            rights.Add(MVC.Comments.Create());

            rights.Add(MVC.Questions.QuestionList());
            rights.Add(MVC.Questions.Create());

            rights.Add(MVC.SpentTime.TimeList());
            rights.Add(MVC.SpentTime.Delete());
            rights.Add(MVC.SpentTime.CreateTaskHours());
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

            if (authorizedUserRights.Can(controller, action))
            {
                return true;
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