using System.Collections.Generic;
using System.Linq;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopTaskRegister : ITaskRegister
    {
        public string Register(string boardName, string taskIdentifier, string folderLink)
        {
            return null;
        }

        public void Unregister(string boardName, string taskIdentifier, string url)
        { }

        public void ChangeResponsible(string boardName, string taskIdentifier, string url, string responsibleUserId)
        { }

        public string DefaultBoardName(string customerName)
        {
            return null;
        }

        public IEnumerable<string> BoardNames()
        {
            return Enumerable.Empty<string>();
        }
    }
}