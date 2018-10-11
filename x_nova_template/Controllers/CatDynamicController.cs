using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Service.Interface;

namespace x_nova_template.Controllers
{
    public class CatDynamicController : Controller
    {
        //
        // GET: /CatDynamic/

        ICategoryRepository _rep;
        public CatDynamicController(ICategoryRepository rep)
        {
            _rep = rep;
        }
        [x_nova_template.Filters.RequireHttps(RequireSecure = false)]
        public ActionResult Index(int id)
        {
            var str = _rep.Categories.Single(x => x.ID == id);
            ViewBag.Body = str.CatDescription;
            return View(str);
        }

    }
}
