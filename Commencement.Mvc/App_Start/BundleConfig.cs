using System.Web;
using System.Web.Optimization;

namespace Commencement.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/easycounter").Include(
                        "~/Scripts/jquery.jqEasyCharCounter.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/qtip").Include(
                        "~/Scripts/jquery.qtip-1.0.0-rc3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables")
                .Include("~/Scripts/DataTables/jquery.dataTables.js")
                .Include("~/Scripts/DataTables/dataTables.bootstrap.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/student-css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/student.css"));

            bundles.Add(
                new StyleBundle("~/Content/DataTables/css/datatables")
                    .Include("~/Content/DataTables/css/buttons.bootstrap.css")                    
                    .Include("~/Content/DataTables/css/dataTables.bootstrap.css")
                    .Include("~/Content/font-awesome.css"));
        }
    }
}
