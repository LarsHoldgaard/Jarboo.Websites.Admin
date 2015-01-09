namespace Jarboo.Admin.BL.Other
{
    public interface ITaskRegister
    {
        string Register(string customerName, string taskTitle, string folderLink);
        void Unregister(string customerName, string taskTitle, string url);
        void ChangeResponsible(string customerName, string taskTitle, string url, string responsibleUserId);
    }
}
