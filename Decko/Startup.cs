using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(Decko.Startup))]

namespace Decko
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/#/Home")
            //});

            //// Use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            ////Need to create these providers if we want to use them.
            ////app.UseMicrosoftAccountAuthentication(new MicrosoftProvider().GetAuthenticationOptions());
            ////app.UseTwitterAuthentication(new TwitterProvider().GetAuthenticationOptions());
            ////app.UseFacebookAuthentication(new FacebookProvider().GetAuthenticationOptions());
            ////app.UseGoogleAuthentication(new GoogleProvider().GetAuthenticationOptions());

            ConfigureAuth(app);
            app.MapSignalR();
            //GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
