using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

using Microsoft.AspNet.Identity.Owin;
using x_nova_template.Extension;
using System.Web.UI;
using x_nova_template.ViewModel;

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using x_nova_template.Service.Repository;
using x_nova_template.Filters;


namespace x_nova_template.Controllers
{
   
    public class HomeController : BaseController
    {

        //private IMenuRepository _mrepo;
        // private IConfigRepository _conf;
        private ApplicationUserManager _userManager;

        public HomeController(ApplicationUserManager userManager)
        {
            _userManager = userManager;

        }
        public HomeController() { }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //public IConfigRepository config { get { return _conf; } private set { _conf = value; } }
        public async Task<ActionResult> Index(string userid = null, string codeAuth = null, string credentialToken = null, int style = 0)
        {

            IConfigRepository _conf = new ConfigRepository();
            IMenuRepository _mrepo = new MenuRepository();
            if (_conf.Options().SelectedIsOnlineID)
            {
                throw new HttpException(410, "Offline");
            }
            var user = await UserManager.FindByIdAsync(userid);
            ViewBag.ConfirmEmail = userid != null && !User.Identity.IsAuthenticated && codeAuth == null ? true : false;
            ViewBag.SetPassword = userid != null && codeAuth != null ? true : false;
            ViewBag.UserName = userid != null && !User.Identity.IsAuthenticated ? user.UserName : "";
            ViewBag.UserId = userid != null && codeAuth != null ? userid : "";
            ViewBag.Code = codeAuth != null && userid != null ? codeAuth : "";
            ViewBag.Style = style;
            ViewBag.Token = credentialToken;
            ViewBag.Titlee = _conf.Options().SiteName;
            var result = _mrepo.Menues.FirstOrDefault(x => x.Url == "Home");
            return View(result);
        }

        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);

            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);/*
            RouteData.Values["culture"] = culture;*/
            return RedirectToAction("Index");
        }
        public ActionResult ImgHolder()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }
    }
}
