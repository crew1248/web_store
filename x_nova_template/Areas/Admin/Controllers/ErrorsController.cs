using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Areas.Admin.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ErrorsController : Controller
    {
        //
        // GET: /Admin/Errors/
        public ActionResult Index(int type) {
            if (type == 500)
            {
                ViewBag.ErrMess = "Ошибка 500! Внутренняя ошибка сервера";
            }
            else if (type == 404) ViewBag.ErrMess = "Ошибка 404! Ресурс не найден!";
            else return Content("Доступ к ресурсу ограничен!");

            //switch (type)
            //{
            //    case 500:  ViewBag.ErrMess = "Ошибка 500! Внутренняя ошибка сервера"; break;
            //    case 404:  ViewBag.ErrMess = "Ошибка 404! Ресурс Не найдено!"; break;
            //    case 403: return Content("Доступ к ресурсу ограничен!"); break;
            //    default: ViewBag.ErrMee = "Ошибка сервера!"; break;
            //}
            return View();
        }
        public ActionResult General(Exception exception)
        {
            //return View("Index", new { type = 500 });
            return Redirect("/Errors?type=500");
        }

        public ActionResult Http404()
        {
            return Redirect("/Errors?type=404");
        }

        public ActionResult Http403()
        {
            return Redirect("/Errors?type=403");
        }
        public ActionResult Offline()
        {
            return View();
        }
        public ActionResult AdminOffline()
        {
            return Redirect("/Admin/Account/LogOn");
        }

    }
}
