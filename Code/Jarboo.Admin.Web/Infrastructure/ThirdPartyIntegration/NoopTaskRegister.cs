using Jarboo.Admin.BL.Other;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopTaskRegister : ITaskRegister
    {
        public string Register(string customerName, string taskIdentifier, string folderLink)
        {
            return null;
        }

        public void Unregister(string customerName, string taskIdentifier, string url)
        { }

        public void ChangeResponsible(string customerName, string taskIdentifier, string url, string responsibleUserId)
        { }
    }
}