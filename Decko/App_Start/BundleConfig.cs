
using System.Web;
using System.Web.Optimization;

namespace Decko
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"
                //, "~/Content/flaty/assets/jquery-ui/jquery-ui.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Content/flaty/assets/jquery-ui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery-validate/jquery.unobtrusive*",
                        "~/Scripts/jquery-validate/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular/angular.js",
                        "~/Scripts/angular/angular-ng-grid.js",
                        "~/Scripts/angular/angular-resource.js",
                        "~/Scripts/angular/angular-route.js",
                        "~/App/plugins/ngStorage.js"));

            bundles.Add(new ScriptBundle("~/bundles/App").Include(
                        "~/App/app.js",
                        "~/App/directives.js",
                        "~/App/services.js",
                        "~/App/factory/GameSvc.js",
                        "~/App/factory/LobbySvc.js",
                        "~/App/controllers/about.js",
                        "~/App/controllers/admin.js",
                        "~/App/controllers/build.js",
                        "~/App/controllers/contact.js",
                        "~/App/controllers/demo.js",
                        "~/App/controllers/lobby.js",
                        "~/App/controllers/game.js",
                        "~/App/controllers/main.js",
                        "~/App/controllers/settings.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                //"~/Content/flaty/assets/bootstrap/bootstrap-responsive.min.css",
                //"~/Content/flaty/assets/normalize/normalize.css",
                "~/Content/flaty/assets/bootstrap/css/bootstrap.min.css",
                "~/Content/flaty/assets/jquery-ui/jquery-ui.min.css",
                //"~/Content/flaty/assets/chosen-bootstrap/chosen.css",
                "~/Content/flaty/assets/font-awesome/css/font-awesome.min.css",
                "~/Content/flaty/assets/gritter/css/jquery.gritter.css",
                "~/Content/flaty/css/flaty.css",
                "~/Content/flaty/css/flaty-responsive.css",
                "~/Content/ng-grid.css",
                "~/Content/site.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/modernizr-*",
                "~/Content/flaty/assets/bootstrap/js/bootstrap.min.js",
                "~/Content/flaty/assets/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/flaty/assets/jquery-cookie/jquery.cookie.js",
                //"~/Content/flaty/assets/chosen-bootstrap/chosen.jquery.js",
                "~/Content/flaty/assets/flot/jquery.flot.js",
                "~/Content/flaty/assets/flot/jquery.flot.pie.js",
                "~/Content/flaty/assets/flot/jquery.flot.stack.js",
                "~/Content/flaty/assets/flot/jquery.flot.crosshair.js",
                "~/Content/flaty/assets/flot/jquery.flot.tooltip.min.js",
                "~/Content/flaty/assets/sparkline/jquery.sparkline.min.js",
                "~/Content/flaty/assets/gritter/js/jquery.gritter.js",
                "~/Scripts/jquery.signalR-2.0.3.js",
                "~/Content/site.js"
            ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}