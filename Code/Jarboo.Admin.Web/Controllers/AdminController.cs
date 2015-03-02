using System;
using System.Threading;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2.Mvc;
using Jarboo.Admin.Web.Infrastructure.ThirdPartyIntegration;

using Nito.AsyncEx.Synchronous;
using Jarboo.Admin.Web.Models;

namespace Jarboo.Admin.Web.Controllers
{
    public partial class AdminController : BaseController
    {
        public virtual ActionResult RequestRefreshToken()
        {
            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(SettingService)).
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

        public virtual ActionResult RevokeToken()
        {
            var result = new AuthorizationCodeMvcApp(this, new AppFlowMetadata(SettingService)).
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


        [HttpGet]
        public virtual ActionResult Settings()
        {
            var model = new SettingsViewModel();
            model.UseGoogleDrive = SmartAdminMvc.Settings.GetValue<bool>("UseGoogleDrive", "");
            model.GoogleApiKey = SmartAdminMvc.Settings.GetValue<string>("GoogleClientId", "");
            model.GoogleApiSecret = SmartAdminMvc.Settings.GetValue<string>("GoogleClientSecret", "");
            model.GoogleTemplatePath = SmartAdminMvc.Settings.GetValue<string>("GoogleDriveTemplatePath", "");
            model.GoogleDrivePath = SmartAdminMvc.Settings.GetValue<string>("GoogleDrivePath", "");
            model.GoogleRefreshToken = SmartAdminMvc.Settings.GetValue<string>("GoogleRefreshToken", "");
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Settings(SettingsViewModel model)
        {
            try
            {
                SmartAdminMvc.Settings.SetValue<bool>("UseGoogleDrive", model.UseGoogleDrive.ToString().ToLower());
                SmartAdminMvc.Settings.SetValue<string>("GoogleClientId", model.GoogleApiKey);
                SmartAdminMvc.Settings.SetValue<string>("GoogleClientSecret", model.GoogleApiSecret);
                SmartAdminMvc.Settings.SetValue<string>("GoogleDriveTemplatePath", model.GoogleTemplatePath);
                SmartAdminMvc.Settings.SetValue<string>("GoogleDrivePath", model.GoogleDrivePath);
                SmartAdminMvc.Settings.SetValue<string>("GoogleRefreshToken", model.GoogleRefreshToken);
                this.AddSuccess("Settings is updated");
            }
            catch (Exception ex)
            {
                this.AddError(ex.Message);
            }
            
            return View(model);
        }
	}
}
