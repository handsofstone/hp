using System.Web;
using System.Web.Optimization;

namespace HP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/tether/tether.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-toggle.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/tether/tether.css",
                      "~/Content/tether/tether-theme-basic.css*",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap2-toggle.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.start.css",
                      "~/Content/autocomplete.css",
                      "~/Content/site.css",
                      "~/Content/justified-nav.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/team").Include(
                "~/Scripts/team.js"));
            bundles.Add(new StyleBundle("~/Content/team").Include(
                "~/Content/team.css"));

            bundles.Add(new ScriptBundle("~/bundles/standings").Include(
                "~/Scripts/standings.js"));
        }
    }
}