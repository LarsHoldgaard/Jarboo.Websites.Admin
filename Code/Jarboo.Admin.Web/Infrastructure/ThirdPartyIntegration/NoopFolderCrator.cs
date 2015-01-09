using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopFolderCrator : IFolderCreator
    {
        public string Create(string customerName, string taskTitle)
        {
            return null;
        }

        public void Delete(string customerName, string taskTitle)
        { }
    }
}