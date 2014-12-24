using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AdminController : BaseController
    {
        public virtual ActionResult Index()
        {
            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(new CancellationTokenSource().Token).Result;

            if (result.Credential != null)
            {
                return Content(result.Credential.Token.RefreshToken);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
	}
}