using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Jarboo.Admin.BL.Services.Interfaces;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class AppFlowMetadata : FlowMetadata
    {
        private readonly Setting _setting;
        public AppFlowMetadata(ISettingService settingService)
        {
            _setting = settingService.GetCurrentSetting();
            _flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets =
                            new ClientSecrets
                                {
                                    ClientId = _setting.GoogleClientId,
                                    ClientSecret = _setting.GoogleClientSecret
                                },
                        Scopes = new[] { DriveService.Scope.Drive },
                        DataStore = MemoryDataStore.Instance
                    });
        }

        private readonly IAuthorizationCodeFlow _flow;
            

        public override string GetUserId(Controller controller)
        {
            return _setting.GoogleLocalUserId;
        }

        public override string AuthCallback
        {
            get
            {
                return "/" + MVC.AuthCallback.Name + "/" + MVC.AuthCallback.Actions.ActionNames.IndexAsync;
            }
        }

        public override IAuthorizationCodeFlow Flow
        {
            get
            {
                return _flow;
            }
        }
    }
}