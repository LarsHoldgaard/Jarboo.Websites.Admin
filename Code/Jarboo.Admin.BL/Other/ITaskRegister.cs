using System.Collections.Generic;

namespace Jarboo.Admin.BL.Other
{
    public interface ITaskRegister
    {
        string DefaultProjectName(string customerName);
        IEnumerable<string> ProjectNames();
        string Register(string projectName, string taskIdentifier, string folderLink);
        void Unregister(string projectName, string taskIdentifier);
        void ChangeResponsible(string projectName, string taskIdentifier, string responsibleUserId);
    }
}
