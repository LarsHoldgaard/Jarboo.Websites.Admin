using System.Threading;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2.Mvc;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;

using Nito.AsyncEx.Synchronous;

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
	}
}