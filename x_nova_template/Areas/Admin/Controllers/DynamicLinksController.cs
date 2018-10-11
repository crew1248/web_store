using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Controllers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class DynamicLinksController : BaseController
    {
        //
        // GET: /Admin/DynamicLinks/
        private IMenuRepository _rep;
        private IOrderRepository _orep;
        public DynamicLinksController(IMenuRepository rep, IOrderRepository orep)
        {
            _rep = rep;
            _orep = orep;
        }

        public ActionResult ViewPage(string urlType, string culture)
        {

            Menu item = null;
            item = (culture != "ru" ? _rep.Menues.Where(x => x.Url == urlType && x.MenuSection > 1).SingleOrDefault() :
                _rep.Menues.Where(x => x.Url == urlType && x.MenuSection <= 1).SingleOrDefault());

            if (urlType == "MakeOrder" || urlType == "Portfolio")
            {
                switch (urlType)
                {
                    case ("MakeOrder"):
                        return PartialView("MakeOrder");
                        break;
                    case ("Portfolio"):
                        return PartialView("Portfolio");
                        break;
                }
            }
            return Json(new
            {
                Body = (culture == "ru" ? item.Body : item.BodyEng),
                viewType = (item.MenuSection == 0 ? "roller" : ""),
                url = urlType
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeOrder()
        {

            return PartialView(new MakeOrderViewModel());
        }

        public ActionResult Portfolio()
        {
            return PartialView();
        }



    }
}
