using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AST_Intranet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "loginRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "loginView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "signupRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "signupView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "forgotpasswordRoute",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "accountsController", action = "forgotpasswordView", id = UrlParameter.Optional }
            );
            /*------------------------------------Views---------------------------------------------------------------*/
            routes.MapRoute(
                name: "dashboardRoute",
                url: "{ controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "DashboardView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EmployeesRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Employees", action = "employeesView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TodayUpdateRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Updates", action = "UpdatesView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PoliciesRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Policies", action = "PoliciesView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ManualsRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Manuals", action = "ManualsView", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "AchievementsRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Achievements", action = "AchievementsView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EventsRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Events", action = "EventsView", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ResourcesRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Resources", action = "blue eyes", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TownHallRoute",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TownHall", action = " eyes", id = UrlParameter.Optional }
            );

        }
    }
}
