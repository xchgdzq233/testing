using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        // the method ASP.NET runtime invokes when the web application starts up.
        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            // ignore any *.axd/* request
            // eg: http://localhost/scriptresource.axd/foo
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // http://localhost/Process
            routes.MapRoute(
                "Process",
                "Process/{action}/{id}",
                new { controller = "Process", action = "List", id = "" }
                );

            // default/catch-all route
            // better be at the end to catch all
            /* eg: 
             *  http://localhost/
             *  http://localhost/home
             *  http://localhost/HoMe/About/
             *  http://localhost/PRoess/List/1/
             */
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = "" } // Parameter defaults
                );
        }
    }
}