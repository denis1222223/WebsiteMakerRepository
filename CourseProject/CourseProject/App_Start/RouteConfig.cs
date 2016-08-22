using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CourseProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AllSites",
                url: "{userName}/all",
                defaults: new { controller = "Sites", action = "Index" }
            );

            routes.MapRoute(
                name: "Edit",
                url: "{userName}/{siteUrl}/edit",
                defaults: new { controller = "Sites", action = "EditSite" }
            );

            routes.MapRoute(
                name: "Details",
                url: "{userName}/{siteUrl}/details",
                defaults: new { controller = "Sites", action = "SiteDetails" }
            );

            routes.MapRoute(
                name: "Delete",
                url: "{userName}/{siteUrl}/delete",
                defaults: new { controller = "Sites", action = "DeleteSite" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "profile/{action}",
                defaults: new { controller = "Manage", action = "Index" }
            );

            routes.MapRoute(
                name: "Account",
                url: "account/{action}",
                defaults: new { controller = "Account" }
            );

            //routes.MapRoute(
            //    name: "Admin",
            //    url: "admin/{action}",
            //    defaults: new { controller = "UserAdmin", action = "Index" }
            //);

            routes.MapRoute(
                name: "User",
                url: "{userName}/{siteUrl}/{pageUrl}/{action}",
                defaults: new { controller = "Sites", action = "Details", pageUrl = "main" }
            );
        }
    }
}
