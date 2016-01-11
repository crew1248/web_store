using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Extension;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;
using System.Configuration;

namespace x_nova_template.Controllers
{
    public class BaseController : Controller
    {
        
        IConfigRepository _conf = new ConfigRepository();
        public BaseController() { }
        public BaseController(IConfigRepository conf) {
            _conf = conf;
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            
            ViewBag.islocalhost = (_conf.Options().SiteName == "localhost" ? true : false);
            ViewBag.firsttime = ConfigurationManager.AppSettings["IsFirstTime"];
           
            string cultureName = null;
           
            /*
            string routeCultureName = RouteData.Values["culture"] as string;
 
            // Attempt to read the culture cookie from Request
            if (routeCultureName == null)
                routeCultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;
            routeCultureName = CultureHelper.GetImplementedCulture(routeCultureName);

            if (RouteData.Values["culture"] as string != routeCultureName && RouteData.Values["controller"]!="ViewPage")
            {
                RouteData.Values["culture"] = routeCultureName.ToLowerInvariant();

                //RouteData.DataTokens["namespaces"]= "x_nova_template.Controllers";
                Response.RedirectToRoute(RouteData.Values);

            }

            */
        
            
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
                cultureCookie = new HttpCookie("_culture");
                cultureCookie.Value = cultureName;
                cultureCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cultureCookie);
            }
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
            
            // Modify current thread's cultures           
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}
