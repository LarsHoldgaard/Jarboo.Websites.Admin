namespace Jarboo.Admin.BL.Other
{
    public interface ITaskRegister
    {
        string Register(string customerName, string taskIdentifier, string folderLink);
        void Unregister(string customerName, string taskIdentifier, string url);
        void ChangeResponsible(string customerName, string taskIdentifier, string url, string responsibleUserId);
    }
}
