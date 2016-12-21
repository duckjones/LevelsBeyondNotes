using LevelsBeyondNotes.Formatters;
using System.Web.Http;

namespace LevelsBeyondNotes
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // This overrides the default behavior of returning an xml content type
            config.Formatters.Add(new BrowserJsonFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
