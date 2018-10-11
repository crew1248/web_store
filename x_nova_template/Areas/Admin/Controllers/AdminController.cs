using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using x_nova_template.Models;
using System.Text.RegularExpressions;

namespace x_nova_template.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/


        public ActionResult Index()
        {
            var input = "admin@admin.ru";
            var name = Regex.Replace(input, @"@\w+.\w+", " ");
            ViewBag.regex = name;
            return View();
        }

        #region -- Robots() Method --
        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
        #endregion


    }
}
