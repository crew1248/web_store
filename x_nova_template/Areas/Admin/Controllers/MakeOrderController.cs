using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Controllers;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class MakeOrderController : BaseController
    {
        //
        // GET: /Admin/MakeOrder/

        public ActionResult Index()
        {
            return View();
        }

    }
}
