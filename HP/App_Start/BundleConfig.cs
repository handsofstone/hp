using System.Web;
using System.Web.Optimization;

namespace HP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery", "//code.jquery.com/jquery-3.2.1.slim.min.js").Include(
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-toggle.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap2-toggle.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.start.css",
                      "~/Content/autocomplete.css",
                      "~/Content/site.css",
                      "~/Content/justified-nav.css"));

            #if DEBUG
            BundleTable.EnableOptimizations = false;
            #else
            BundleTable.EnableOptimizations = true;
            #endif
            BundleTable.Bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/Content/materials", "https://fonts.googleapis.com/icon?family=Material+Icons").Include(
                        "~/Content/material_icon.css"));
            bundles.Add(new ScriptBundle("~/bundles/fontawesome", "use.fontawesome.com/releases/v5.0.8/js/fontawesome.js").Include(
                        "~/Scripts/fontawesome.js"));
            bundles.Add(new ScriptBundle("~/bundles/solid", "use.fontawesome.com/releases/v5.0.8/js/solid.js").Include(
                        "~/Scripts/fa-solid.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                      "~/Scripts/DataTables/jquery.dataTables.js",
                      "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/team").Include(
                "~/Scripts/team.js"));
            bundles.Add(new StyleBundle("~/Content/team").Include(
                "~/Content/team.css"));

            bundles.Add(new ScriptBundle("~/bundles/standings").Include(
                "~/Scripts/standings.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                "~/Scripts/flot/jquery.flot.js",
                "~/Scripts/flot/jquery.flot.*"));
            bundles.Add(new ScriptBundle("~/bundles/popper", "//cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js").Include(
                "~/Scripts/umd/popper.js"));
        }
    }
}