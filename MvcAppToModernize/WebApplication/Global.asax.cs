// This is the main application class that serves as the entry point for the ASP.NET MVC application
// It inherits from HttpApplication which provides the base functionality for handling HTTP requests
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Application_Start is called once when the application first starts up
        // It handles all the necessary configuration and initialization
        protected void Application_Start()
        {
            // Registers all areas in the MVC application to enable area-based routing
            AreaRegistration.RegisterAllAreas();
            
            // Configures and registers the routing rules for the application
            // This determines how URLs are mapped to controller actions
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Initializes the Unity dependency injection container
            // This sets up dependency injection for the application
            UnityConfig.RegisterUnity();
        }
    }
}
