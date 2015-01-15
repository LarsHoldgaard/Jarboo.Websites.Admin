namespace Jarboo.Admin.BL.Other
{
    public interface IFolderCreator
    {
        string Create(string customerName, string taskIdentifier);
        void Delete(string customerName, string taskIdentifier);
    }
}
