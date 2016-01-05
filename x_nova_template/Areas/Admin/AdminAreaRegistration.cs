using System.Web.Mvc;

namespace x_nova_template.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            //context.MapRoute(
            //    "define",
            //    "{controller}/{action}/{*id}",
            //     new { controller = "Post", action = "LogOff", id = UrlParameter.Optional },
            //    namespaces: new[] { "x_nova_template.Areas.Admin.Controllers" }

            //);
            context.MapRoute(
                   "", // Route name
                   "Admin/Account/LogOff", // URL with parameters
                   new { controller = "Account", action = "LogOff", id = UrlParameter.Optional },
                   namespaces: new[] { "x_nova_template.Areas.Admin.Controllers" }// Parameter defaults
               );

            context.MapRoute(
                   "", // Route name
                   "Admin/Account/ChangePassword", // URL with parameters
                   new { controller = "Account", action = "ChangePassword", id = UrlParameter.Optional },
                   namespaces: new[] { "x_nova_template.Areas.Admin.Controllers" }// Parameter defaults
               );

            //context.MapRoute(
            //    "log",
            //    "Admin",
            //    new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new[] { "x_nova_template.Areas.Admin.Controllers" }

            //);
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "x_nova_template.Areas.Admin.Controllers" }

            );
        }
    }
}
