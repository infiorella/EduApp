using System.Web;
using System.Web.Optimization;

namespace EduApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Base
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Login/css").Include(
                      "~/Content/css/main/app.css",
                      "~/Content/css/pages/auth.css"));

            //Docente
            bundles.Add(new StyleBundle("~/Content/Docente/css").Include(
                       "~/Content/bootstrap/dist/css/bootstrap.min.css",
                       "~/Content/css/main/app-dark.css",
                       "~/Content/css/main/app.css",
                       "~/Content/css/shared/iconly.css"));

            bundles.Add(new Bundle("~/Content/Docente/js").Include(
                   "~/Content/js/bootstrap.js",
                    "~/Content/js/app.js"));


            bundles.Add(new Bundle("~/Content/Charts/js").Include(
                   "~/Content/extensions/chart.js/Chart.min.js",
                   "~/Content/js/pages/ui-chartjs.js"));


            bundles.Add(new Bundle("~/Docente/Dashoard/js").Include(
                    "~/Content/extensions/apexcharts/apexcharts.min.js",
                    "~/Content/js/pages/dashboard.js"));

            bundles.Add(new Bundle("~/Content/ApexCharts/js").Include(
                   "~/Content/extensions/apexcharts/apexcharts.min.js",
                   "~/Content/js/pages/docente-charts.js"));

            bundles.Add(new Bundle("~/Content/Datatable/js").Include(
                   "~/Content/extensions/jquery/jquery.min.js",
                   "~/Content/js/pages/datatables.js"));

            bundles.Add(new Bundle("~/Content/Datatable/simple/js").Include(
                  "~/Content/extensions/simple-datatables/umd/simple-datatables.js",
                  "~/Content/js/pages/simple-datatables.js"));

            //Chart detalle alumno
            bundles.Add(new Bundle("~/Content/ApexCharts/DetalleAlumno/js").Include(
                   "~/Content/extensions/apexcharts/apexcharts.min.js",
                   "~/Content/js/pages/detalle-alumno-chart.js"));

            //ChartActividades
            bundles.Add(new Bundle("~/Content/ApexCharts/Actividades/js").Include(
                   "~/Content/extensions/apexcharts/apexcharts.min.js",
                   "~/Content/js/pages/actividades.js"));




            //Estudiante
            bundles.Add(new StyleBundle("~/Content/Estudiante/css").Include(
                       "~/Content/bootstrap/dist/css/bootstrap.min.css",
                       "~/Content/css/main/app-dark.css",
                       "~/Content/css/main/app.css",
                       "~/Content/css/shared/iconly.css"));

            bundles.Add(new Bundle("~/Content/Estudiante/js").Include(
                   "~/Content/js/bootstrap.js",
                    "~/Content/js/app.js"));


            bundles.Add(new Bundle("~/Content/Charts/js").Include(
                   "~/Content/extensions/chart.js/Chart.min.js",
                   "~/Content/js/pages/ui-chartjs.js"));
        }
    }
}
