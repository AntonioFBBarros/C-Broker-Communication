using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using GlobalAPI.Auth;
using GlobalAPI.Exceptions;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(GlobalAPI.App_Start.Startup))]

namespace GlobalAPI.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                  name: "DefaultApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { id = RouteParameter.Optional }
             );

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            app.UseCors(CorsOptions.AllowAll);

            ActivateTokenGeneration(app);

            app.UseWebApi(config);
        }

        private void ActivateTokenGeneration(IAppBuilder app)
        {
            var tokenConfigurationOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                Provider = new TokenProvider()
            };
            app.UseOAuthAuthorizationServer(tokenConfigurationOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
