using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _7Qualityequip.Controllers
{
    public class ActionsController : Controller
    {
        //
        // GET: /Actions/

        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult GetAction(int id){
            return View();
        }

    }
}
