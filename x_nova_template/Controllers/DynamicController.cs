using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Controllers;
using x_nova_template.Service.Interface;

namespace x_nova_template.Controllers
{

    public class DynamicController : BaseController
    {
        //
        // GET: /Dynamic/

        IMenuRepository _rep;
        public DynamicController(IMenuRepository rep)
        {
            _rep = rep;
        }
        [x_nova_template.Filters.RequireHttps(RequireSecure = false)]
        [OutputCache(Duration = 60, VaryByParam = "routes")]
        public ActionResult Indexx(string routes)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;

            var str = _rep.Menues.Single(x => x.Url == routes);

            return View(str);
        }



    }
}
