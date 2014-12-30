using Jarboo.Admin.BL.ThirdParty;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopTaskRegister : ITaskRegister
    {
        public string Register(string customerName, string taskTitle)
        {
            return null;
        }

        public void Unregister(string customerName, string taskTitle, string url)
        { }

        public void ChangeResponsible(string customerName, string taskTitle, string url, string responsibleUserId)
        { }
    }
}