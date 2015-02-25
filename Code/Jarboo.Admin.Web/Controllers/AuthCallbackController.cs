using Google.Apis.Auth.OAuth2.Mvc;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;
using Ninject;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        [Inject]
        public ISettingService SettingService { get; set; }
        protected override FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(SettingService); }
        }
    }
}