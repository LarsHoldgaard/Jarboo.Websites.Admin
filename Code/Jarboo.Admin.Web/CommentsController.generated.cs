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
    public partial class CommentsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CommentsController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected CommentsController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult CommentList()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CommentList);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Create()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public CommentsController Actions { get { return MVC.Comments; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Comments";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Comments";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string CommentList = "CommentList";
            public readonly string Create = "Create";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string CommentList = "CommentList";
            public const string Create = "Create";
        }


        static readonly ActionParamsClass_CommentList s_params_CommentList = new ActionParamsClass_CommentList();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CommentList CommentListParams { get { return s_params_CommentList; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CommentList
        {
            public readonly string taskId = "taskId";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string taskId = "taskId";
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
                public readonly string _AddCommentForm = "_AddCommentForm";
                public readonly string _ListComment = "_ListComment";
            }
            public readonly string _AddCommentForm = "~/Views/Comments/_AddCommentForm.cshtml";
            public readonly string _ListComment = "~/Views/Comments/_ListComment.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_CommentsController : Jarboo.Admin.Web.Controllers.CommentsController
    {
        public T4MVC_CommentsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void CommentListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int taskId);

        [NonAction]
        public override System.Web.Mvc.ActionResult CommentList(int taskId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CommentList);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "taskId", taskId);
            CommentListOverride(callInfo, taskId);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int taskId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create(int taskId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "taskId", taskId);
            CreateOverride(callInfo, taskId);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Jarboo.Admin.Web.Models.Comment.CommentViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult Create(Jarboo.Admin.Web.Models.Comment.CommentViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CreateOverride(callInfo, model);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
