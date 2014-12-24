using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Util.Store;

namespace Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration
{
    public class AppFlowMetadata : FlowMetadata
    {
        public static string UserId = "user";

        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets =
                            new ClientSecrets
                                {
                                    ClientId = Configuration.GoogleClientId,
                                    ClientSecret = Configuration.GoogleClientSecret
                                },
                        Scopes = new[] { DriveService.Scope.Drive },
                        DataStore = MemoryDataStore.Instance
                    });

        public override string GetUserId(Controller controller)
        {
            return UserId;
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
                return flow;
            }
        }
    }
}