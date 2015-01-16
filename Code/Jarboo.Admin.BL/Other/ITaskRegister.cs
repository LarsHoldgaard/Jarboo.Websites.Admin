using System.Collections.Generic;

namespace Jarboo.Admin.BL.Other
{
    public interface ITaskRegister
    {
        string DefaultBoardName(string customerName);
        IEnumerable<string> BoardNames();
        string Register(string boardName, string taskIdentifier, string folderLink);
        void Unregister(string boardName, string taskIdentifier, string url);
        void ChangeResponsible(string boardName, string taskIdentifier, string url, string responsibleUserId);
    }
}
