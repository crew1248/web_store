using System.Web;
using System.Web.Optimization;

namespace x_nova_template
{
    public class BundleConfig
    {
        
        //Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.4.min.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(

                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxauth").Include(
                      "~/Scripts/ajax-auth.js"));
            bundles.Add(new ScriptBundle("~/bundles/ajaxset").Include(
                     "~/Scripts/ajax-set.js"));
            bundles.Add(new ScriptBundle("~/bundles/ajaxreg").Include(
                      "~/Scripts/ajax-reg.js"));
            bundles.Add(new ScriptBundle("~/bundles/ajaxrec").Include(
                      "~/Scripts/ajax-rec.js"));
            //main js
             bundles.Add(new ScriptBundle("~/bundles/sitejs").Include(
                  
             "~/Scripts/jquery.jcarousel.min.js",
            
            "~/Scripts/site.js",
            "~/Scripts/modalEffects.js", 
           
          
            "~/Scripts/jquery.signalR-2.2.0.min.js"  
            ));
            bundles.Add(new ScriptBundle("~/bundles/tt").Include(
                //"~/Scripts/jquery.jcarousel.min.js",
                
                 "~/Scripts/modalEffects.js",
                "~/Scripts/comm.js",
                "~/Scripts/menu.js",
                "~/Scripts/site.js",
                "~/Scripts/jquery.signalR-2.2.0.min.js"
           
                ));
            //main css
            bundles.Add(new StyleBundle("~/Content/authcss").Include(
                 "~/Content/auth.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
              "~/Content/site.css",
              "~/font-awesome/css/font-awesome.min.css",

              //"~/Content/kendo/2013.3.1324/kendo.common.min.css",
              //"~/Content/kendo/2013.3.1324/kendo.default.min.css",
                "~/Content/menupush/default.css",
                "~/Content/menupush/component.css",
              "~/Content/component.css",
                "~/Content/btns.css",
                "~/Content/listview.css",
              "~/Content/Livechat.css"
              //"~/Content/jquery.mCustomScrollbar.css"

              ));
            bundles.Add(new ScriptBundle("~/bundles/mobile").Include(
                 "~/Scripts/jquery-2.1.4.min.js",
                  "~/Scripts/jquery.validate*",
                  "~/Scripts/jquery.unobtrusive-ajax.min.js",

                   "~/Scripts/kendo/2015.1.429/kendo.mobile.min.js"


                ));
            bundles.Add(new ScriptBundle("~/bundles/mobilesite").Include(
                "~/Scripts/comm.js",
                "~/Scripts/site.js"
                ));
            bundles.Add(new StyleBundle("~/Content/mobilecss").Include(
               "~/Content/kendo/2015.1.429/kendo.mobile.all.min.css",
                "~/Content/site.css",
                "~/font-awesome/css/font-awesome.min.css"

               ));
            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
                "~/Content/site.css"
               ));
            bundles.Add(new ScriptBundle("~/bundles/map").Include(
                "~/Scripts/raphael.min.js",
                "~/Scripts/scale-raphael.js",
                "~/Scripts/svg-map.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/zoomjs").Include(

                 "~/Scripts/plugin/tiksluszoom.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/zoomcss").Include(
                "~/Content/plugin/tiksluszoom.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                "~/Scripts/jquery.signalR-2.2.0.min.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/livechat").Include(
                 "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                "~/Scripts/xn-livechat.js"

                ));
            bundles.Add(new StyleBundle("~/Content/livechat").Include(
                "~/Content/jquery.mCustomScrollbar.css"
               ));
            /*    KENDO BUNDLES   */

            bundles.Add(new ScriptBundle("~/bundles/kjs").Include(
                "~/Scripts/kendo/2015.2.624/kendo.web.min.js",
                "~/Scripts/kendo/2015.2.624/kendo.aspnetmvc.min.js",
                 "~/Scripts/cultures/kendo.culture.ru.js",
                "~/Scripts/cultures/kendo.culture.ru-RU.js"
                ));
            bundles.Add(new StyleBundle("~/Content/kcss").Include(
                            "~/Content/kendo/2015.2.624/kendo.common-material.min.css",
                        "~/Content/kendo/2015.2.624/kendo.material.min.css",
                        "~/font-awesome/css/font-awesome.min.css"
                        ));

            /*    ADMIN BUNDLES   */

            bundles.Add(new ScriptBundle("~/bundles/admjs").Include(
                      "~/Scripts/adm.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/admcss").Include(

                "~/Content/tiptip.css",
                "~/Content/Admin.css"
              ));
            bundles.Add(new StyleBundle("~/Content/admlogincss").Include(
                   "~/Content/admlogin.css"
                 ));
          
        }
    }
}
