using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jarboo.Admin.BL.ThirdParty;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopFolderCrator : IFolderCreator
    {
        public void Create(string customerName, string taskTitle)
        { }

        public void Delete(string customerName, string taskTitle)
        { }
    }
}