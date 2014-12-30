using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;

using Jarboo.Admin.Web.Infrastructure;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;

using Nito.AsyncEx.Synchronous;

using TrelloNet;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AdminController : BaseController
    {
        public virtual ActionResult RequestRefreshToken()
        {
            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(new CancellationTokenSource().Token).WaitAndUnwrapException();

            if (result.Credential != null)
            {
                if (!string.IsNullOrEmpty(result.Credential.Token.RefreshToken))
                {
                    return Content(result.Credential.Token.RefreshToken);
                }
                else
                {
                    return Content("Server didn't return refresh token. To reissue token you should disconnect app from google drive first.");
                }
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        public virtual ActionResult TrelloId(string id)
        {
            if (string.IsNullOrEmpty(Configuration.TrelloApiKey) || string.IsNullOrEmpty(Configuration.TrelloToken))
            {
                throw new ApplicationException("Missing trello configuration");
            }

            var trello = new Trello(Configuration.TrelloApiKey);
            trello.Authorize(Configuration.TrelloToken);

            var members = trello.Members.Search(id);
            if (members == null)
            {
                return Content("Member not found");
            }

            var member = members.FirstOrDefault();
            if (member == null)
            {
                return Content("Member not found");
            }

            return Content(member.Id);
        }
	}
}