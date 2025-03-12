using System.Web;
using System.Web.Optimization;

namespace AST_Intranet
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            // CSS Bundle for Bootstrap and Site styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/css/AchievementsStyles.css",
                      "~/Content/css/EmployeesStyles.css",
                      "~/Content/css/EventsStyles.css",
                      "~/Content/css/HomepageStyles.css",
                      "~/Content/css/ManualsStyles.css",
                      "~/Content/css/PoliciesStyles.css",
                      "~/Content/css/UpdatesStyles.css",
                      // These files do not exist yet, but will be created in the future
                      "~/Content/css/ResourcesStyles.css",
                      "~/Content/css/TownHallMeetingStyles.css"));

            // JS Bundle for site-specific JS files
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/js/AchievementsScript.js",
                      "~/Scripts/js/EmployeesScript.js",
                      "~/Scripts/js/EventsScript.js",
                      "~/Scripts/js/HomepageScript.js",
                      "~/Scripts/js/ManualsScript.js",
                      "~/Scripts/js/PoliciesScript.js",
                      "~/Scripts/js/UpdatesScript.js",
                      // These files do not exist yet, but will be created in the future
                      "~/Scripts/js/ResourcesScript.js",
                      "~/Scripts/js/TownHallMeetingScript.js"));
        }
    }
}
