using System;

using Jarboo.Admin.Web;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;

[assembly: OwinStartup(typeof(OwinConfig))]

namespace Jarboo.Admin.Web
{
    public class OwinConfig
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.UseCookieAuthentication(new CookieAuthenticationOptions()
                                                {
                                                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                                                    LoginPath = new PathString("/Accounts/Login"),
                                                    ExpireTimeSpan = new TimeSpan(30, 0, 0, 0)
                                                });
        }
    }
}