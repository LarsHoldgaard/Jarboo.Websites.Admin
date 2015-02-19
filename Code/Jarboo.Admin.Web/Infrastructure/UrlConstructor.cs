using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Web.Infrastructure
{
    public class UrlConstructor : IUrlConstructor
    {
        public string TaskView(int taskId)
        {
            var requestContext = Helper.GetRequestContext();
            return new UrlHelper(requestContext).Action(MVC.Tasks.View(taskId), requestContext.HttpContext.Request.Url.Scheme);
        }
    }
}