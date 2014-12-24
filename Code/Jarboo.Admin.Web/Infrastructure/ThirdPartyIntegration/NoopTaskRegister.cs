using Jarboo.Admin.BL.ThirdParty;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class NoopTaskRegister : ITaskRegister
    {
        public void Register(string customerName, string taskTitle)
        { }

        public void Unregister(string customerName, string taskTitle)
        { }
    }
}