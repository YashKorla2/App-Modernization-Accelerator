// This class handles the configuration of URL routing in the ASP.NET MVC application
// It defines how URLs are mapped to controller actions
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        // Registers all application routes in the route table
        // Parameter routes: Collection that stores all registered routes
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Ignores requests to .axd files (typically used for WebResource.axd, ScriptResource.axd etc)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Maps the default route pattern for the application
            // Format: /[Controller]/[Action]/[ID]
            // Example: /Product/Details/5
            // If no values provided, defaults to Product controller, Index action
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
