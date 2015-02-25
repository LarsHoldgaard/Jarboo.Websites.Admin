// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Jarboo.Admin.Web.Controllers
{
    public partial class TasksController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TasksController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected TasksController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult View()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.View);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Steps()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Steps);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult NextStep()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextStep);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Delete()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult List()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ListData()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListData);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult NextTask()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextTask);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddHours()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddHours);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TasksController Actions { get { return MVC.Tasks; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Tasks";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Tasks";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string View = "View";
            public readonly string Create = "Create";
            public readonly string Edit = "Edit";
            public readonly string Steps = "Steps";
            public readonly string NextStep = "NextStep";
            public readonly string Delete = "Delete";
            public readonly string List = "List";
            public readonly string ListConfig = "ListConfig";
            public readonly string ListData = "ListData";
            public readonly string NextTask = "NextTask";
            public readonly string AddHours = "AddHours";
            public readonly string TasksPerDayChartData = "TasksPerDayChartData";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string View = "View";
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string Steps = "Steps";
            public const string NextStep = "NextStep";
            public const string Delete = "Delete";
            public const string List = "List";
            public const string ListConfig = "ListConfig";
            public const string ListData = "ListData";
            public const string NextTask = "NextTask";
            public const string AddHours = "AddHours";
            public const string TasksPerDayChartData = "TasksPerDayChartData";
        }


        static readonly ActionParamsClass_View s_params_View = new ActionParamsClass_View();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_View ViewParams { get { return s_params_View; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_View
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string projectId = "projectId";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string id = "id";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Steps s_params_Steps = new ActionParamsClass_Steps();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Steps StepsParams { get { return s_params_Steps; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Steps
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_NextStep s_params_NextStep = new ActionParamsClass_NextStep();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_NextStep NextStepParams { get { return s_params_NextStep; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_NextStep
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string id = "id";
            public readonly string returnUrl = "returnUrl";
        }
        static readonly ActionParamsClass_List s_params_List = new ActionParamsClass_List();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_List ListParams { get { return s_params_List; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_List
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_ListConfig s_params_ListConfig = new ActionParamsClass_ListConfig();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListConfig ListConfigParams { get { return s_params_ListConfig; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListConfig
        {
            public readonly string showProject = "showProject";
            public readonly string sorting = "sorting";
        }
        static readonly ActionParamsClass_ListData s_params_ListData = new ActionParamsClass_ListData();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ListData ListDataParams { get { return s_params_ListData; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ListData
        {
            public readonly string request = "request";
            public readonly string taskFilter = "taskFilter";
        }
        static readonly ActionParamsClass_NextTask s_params_NextTask = new ActionParamsClass_NextTask();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_NextTask NextTaskParams { get { return s_params_NextTask; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_NextTask
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_AddHours s_params_AddHours = new ActionParamsClass_AddHours();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddHours AddHoursParams { get { return s_params_AddHours; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddHours
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _AddHours = "_AddHours";
                public readonly string _Form = "_Form";
                public readonly string _NextStep = "_NextStep";
                public readonly string Create = "Create";
                public readonly string Edit = "Edit";
                public readonly string Index = "Index";
                public readonly string List = "List";
                public readonly string NextTask = "NextTask";
                public readonly string Steps = "Steps";
                public readonly string View = "View";
            }
            public readonly string _AddHours = "~/Views/Tasks/_AddHours.cshtml";
            public readonly string _Form = "~/Views/Tasks/_Form.cshtml";
            public readonly string _NextStep = "~/Views/Tasks/_NextStep.cshtml";
            public readonly string Create = "~/Views/Tasks/Create.cshtml";
            public readonly string Edit = "~/Views/Tasks/Edit.cshtml";
            public readonly string Index = "~/Views/Tasks/Index.cshtml";
            public readonly string List = "~/Views/Tasks/List.cshtml";
            public readonly string NextTask = "~/Views/Tasks/NextTask.cshtml";
            public readonly string Steps = "~/Views/Tasks/Steps.cshtml";
            public readonly string View = "~/Views/Tasks/View.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_TasksController : Jarboo.Admin.Web.Controllers.TasksController
    {
        public T4MVC_TasksController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ViewOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? id);

        [NonAction]
        public override System.Web.Mvc.ActionResult View(int? id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.View);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ViewOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? projectId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create(int? projectId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "projectId", projectId);
            CreateOverride(callInfo, projectId);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Edit(int? id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Jarboo.Admin.BL.Models.TaskEdit model);

        [NonAction]
        public override System.Web.Mvc.ActionResult Edit(Jarboo.Admin.BL.Models.TaskEdit model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void StepsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? id);

        [NonAction]
        public override System.Web.Mvc.ActionResult Steps(int? id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Steps);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            StepsOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void NextStepOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Jarboo.Admin.BL.Models.TaskNextStep model);

        [NonAction]
        public override System.Web.Mvc.ActionResult NextStep(Jarboo.Admin.BL.Models.TaskNextStep model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextStep);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            NextStepOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void DeleteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int id, string returnUrl);

        [NonAction]
        public override System.Web.Mvc.ActionResult Delete(int id, string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            DeleteOverride(callInfo, id, returnUrl);
            return callInfo;
        }

        [NonAction]
        partial void ListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Jarboo.Admin.Web.Models.Task.TasksListViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult List(Jarboo.Admin.Web.Models.Task.TasksListViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ListOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void ListConfigOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool showProject, Jarboo.Admin.Web.Models.Task.TaskSorting sorting);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListConfig(bool showProject, Jarboo.Admin.Web.Models.Task.TaskSorting sorting)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListConfig);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "showProject", showProject);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "sorting", sorting);
            ListConfigOverride(callInfo, showProject, sorting);
            return callInfo;
        }

        [NonAction]
        partial void ListDataOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, DataTables.Mvc.IDataTablesRequest request, Jarboo.Admin.BL.Filters.TaskFilter taskFilter);

        [NonAction]
        public override System.Web.Mvc.ActionResult ListData(DataTables.Mvc.IDataTablesRequest request, Jarboo.Admin.BL.Filters.TaskFilter taskFilter)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ListData);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "request", request);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "taskFilter", taskFilter);
            ListDataOverride(callInfo, request, taskFilter);
            return callInfo;
        }

        [NonAction]
        partial void NextTaskOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int? id);

        [NonAction]
        public override System.Web.Mvc.ActionResult NextTask(int? id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextTask);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            NextTaskOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void AddHoursOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Jarboo.Admin.BL.Models.SpentTimeOnTask model);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddHours(Jarboo.Admin.BL.Models.SpentTimeOnTask model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddHours);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            AddHoursOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void TasksPerDayChartDataOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult TasksPerDayChartData()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TasksPerDayChartData);
            TasksPerDayChartDataOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
