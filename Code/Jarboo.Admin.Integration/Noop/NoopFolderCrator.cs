using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Integration.Noop
{
    public class NoopFolderCrator : IFolderCreator
    {
        public string Create(string customerName, string taskIdentifier)
        {
            return null;
        }

        public void Delete(string customerName, string taskIdentifier)
        { }
    }
}