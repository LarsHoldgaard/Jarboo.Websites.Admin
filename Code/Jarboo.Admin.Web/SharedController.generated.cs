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
namespace T4MVC
{
    public class SharedController
    {

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
                public readonly string _Aside = "_Aside";
                public readonly string _CurrentUser = "_CurrentUser";
                public readonly string _Footer = "_Footer";
                public readonly string _Header = "_Header";
                public readonly string _Layout = "_Layout";
                public readonly string _Layout_ = "_Layout_";
                public readonly string _Ribbon = "_Ribbon";
                public readonly string _Scripts = "_Scripts";
                public readonly string _Tiles = "_Tiles";
                public readonly string _TopRight = "_TopRight";
                public readonly string DeleteBtnForm = "DeleteBtnForm";
                public readonly string Error = "Error";
                public readonly string Messages = "Messages";
                public readonly string TopMenu = "TopMenu";
            }
            public readonly string _Aside = "~/Views/Shared/_Aside.cshtml";
            public readonly string _CurrentUser = "~/Views/Shared/_CurrentUser.cshtml";
            public readonly string _Footer = "~/Views/Shared/_Footer.cshtml";
            public readonly string _Header = "~/Views/Shared/_Header.cshtml";
            public readonly string _Layout = "~/Views/Shared/_Layout.cshtml";
            public readonly string _Layout_ = "~/Views/Shared/_Layout_.cshtml";
            public readonly string _Ribbon = "~/Views/Shared/_Ribbon.cshtml";
            public readonly string _Scripts = "~/Views/Shared/_Scripts.cshtml";
            public readonly string _Tiles = "~/Views/Shared/_Tiles.cshtml";
            public readonly string _TopRight = "~/Views/Shared/_TopRight.cshtml";
            public readonly string DeleteBtnForm = "~/Views/Shared/DeleteBtnForm.cshtml";
            public readonly string Error = "~/Views/Shared/Error.cshtml";
            public readonly string Messages = "~/Views/Shared/Messages.cshtml";
            public readonly string TopMenu = "~/Views/Shared/TopMenu.cshtml";
        }
    }

}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
