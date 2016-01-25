using x_nova_template.Areas.Admin.Controllers;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace x_nova_template
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
              name: "sitemap.xml",
              url: "sitemap.xml",
              defaults: new { controller = "Config", action = "SitemapXml" },
              namespaces: new[] { "x_nova.Areas.Admin.Controllers" }
          );

            routes.MapRoute(
                name: "robots.txt",
                url: "robots.txt",
                defaults: new { controller = "Config", action = "RobotsText" },
                namespaces: new[] { "x_nova.Controllers" }
            );


            routes.MapRoute(
               name: "news",
               url: "Post",
               defaults: new { controller = "Post", action = "Index" },
               namespaces: new[] { "x_nova_template.Controllers" }

           );
            routes.MapRoute(
             name: "photog",
             url: "Photogallery",
             defaults: new { controller = "PhotoGallery", action = "Index" },
             namespaces: new[] { "x_nova_template.Controllers" }

         );
            routes.MapRoute(
               name: "login",
               url: "Account/Login",
               defaults: new { controller = "Account", action = "Login" },
               namespaces: new[] { "x_nova_template.Controllers" }

           );
         

            /*      routes.MapRoute(
         name: "port",
         url: "{culture}/{controller}",
         defaults: new { culture=CultureHelper.GetDefaultCulture(), controller = "Portfolio", action = "Index" },
         namespaces: new[] { "Qualityequip.Controllers" }
        );
                  routes.MapRoute(
      name: "order",
      url: "{culture}/{controller}",
      defaults: new { culture = CultureHelper.GetDefaultCulture(), controller = "MakeOrder", action = "Index" },
      namespaces: new[] { "Qualityequip.Controllers" }
      );*/
            IMenuRepository _repository = new MenuRepository();
            string[] excludeUrl = { "Post", "tobuy","Home","PhotoGallery" };
            foreach (var item in _repository.Menues.Where(x => !excludeUrl.Contains(x.Url)))
            {

                if (item.ParentId > 0)
                {
                    routes.MapRoute(
                    name: "Menu" + item.Id,
                    url: MenuController.GetMenuUrl(item.ParentId) + "/" + item.Url,
                    defaults: new { controller = "Dynamic", action = "Indexx", routes = item.Url },
                    namespaces: new[] { "x_nova_template.Controllers" }
                    );
                }
                else
                {
                    routes.MapRoute(
                    name: "Menu" + item.Id,
                    url: item.Url,
                    defaults: new { controller = "Dynamic", action = "Indexx", routes = item.Url },
                    namespaces: new[] { "x_nova_template.Controllers" }
                    );
                }


            }
            routes.MapRoute(
                name: "home",
                url: "main",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "x_nova_template.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Product", action = "ProdList", id=UrlParameter.Optional},
                namespaces: new[] { "x_nova_template.Controllers" }
            );
        }
    }
}
