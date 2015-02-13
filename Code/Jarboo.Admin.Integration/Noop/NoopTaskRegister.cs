using System.Collections.Generic;
using System.Linq;

using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Integration.Noop
{
    public class NoopTaskRegister : ITaskRegister
    {
        public string DefaultProjectName(string customerName)
        {
            return "Jarboo";
        }

        public IEnumerable<string> ProjectNames()
        {
            return Enumerable.Empty<string>();
        }

        public string Register(string projectName, string taskIdentifier, string folderLink)
        {
            return "#";
        }

        public void Unregister(string projectName, string taskIdentifier)
        {
        }

        public void ChangeResponsible(string projectName, string taskIdentifier, string responsibleUserId)
        {
        }
    }
}