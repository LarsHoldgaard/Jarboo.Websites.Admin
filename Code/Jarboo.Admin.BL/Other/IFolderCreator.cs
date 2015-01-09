namespace Jarboo.Admin.BL.Other
{
    public interface IFolderCreator
    {
        string Create(string customerName, string taskTitle);
        void Delete(string customerName, string taskTitle);
    }
}
