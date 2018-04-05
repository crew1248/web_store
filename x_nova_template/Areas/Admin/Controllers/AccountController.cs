using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using x_nova_template.Models;
using x_nova_template.ViewModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;

namespace x_nova_template.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            //a
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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


        public ActionResult GetUsers()
        {
            var users = UserManager.Users.ToList();
            return View(users);
        }
        //
        // GET: /Account/Login   fff
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AdmLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var formattedEmail = model.Email + "@admin.ru";
            ApplicationUser user = await UserManager.FindByEmailAsync(formattedEmail);
            
            if (user == null)
            {
                ModelState.AddModelError("", "Неправильные логин или пароль !");
                return View(model);
            }
            var adminId = user.Id;
            var pass = UserManager.CheckPassword(user, model.Password);
            //UserManager.SetTwoFactorEnabled(adminId, false);
            //if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //{

            //    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Подтверждение аккаунта - новое");

            //    ViewBag.errorMessage = "Вы должны подтвердить свой аккаунт, чтобы войти !";
            //    return View("Error");
            //}

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true

            bool confirmed =await UserManager.IsEmailConfirmedAsync(user.Id);
            var confirmed1 = await UserManager.IsLockedOutAsync(user.Id);
            bool confirmed2 = await UserManager.GetTwoFactorEnabledAsync(user.Id);
            var confirmed3 = user.UserName;
            if (!confirmed)
            {
                //UserManager.SetTwoFactorEnabled(adminId, true);
               
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var confirmResult = UserManager.ConfirmEmail(user.Id, code);
                if (!confirmResult.Succeeded) { ModelState.AddModelError("", "Аккаунт не удалось подтвердить."); return View(); }
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index","Admin",null);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                     ModelState.AddModelError("", "Неудачная попытка входа.!");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
            }

        [HttpGet]
        [Authorize(Roles="admin")]
        public async Task<ActionResult> RemoveUser(string email) {
            if (ModelState.IsValid)
            {

                if (email == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByEmailAsync(email);
                var logins = user.Logins;

                foreach (var login in logins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                var rolesForUser = await UserManager.GetRolesAsync(user.Id);

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);
            }
           // UserManager.RemoveLoginAsync(user.Id,user.lo)
            return RedirectToAction("GetUsers");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(AdmChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index","Admin", new { Message = "пароль успешно изменен !" });
            }
            AddErrors(result);
            return View(model);
        }

      
      
        //
        // POST: /Account/LogOff
        
        
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            ConsultantController.SetConsultOffline();
            return Redirect("/");
        }
      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        /*    KENDO    GRID    */


        //public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    DataSourceResult result = UserManager.Users.ToDataSourceResult(request, o => new CategoryViewModel
        //    {
        //        CatDescription = o.CatDescription,
        //        CatType = o.CatType,
        //        CategoryName = o.CategoryName,
        //        Sequance = o.Sequance,
        //        ID = o.ID

        //    });
        //    return Json(result);
        //}
        //public JsonResult GetCategories()
        //{
        //    var jsonResult = Json(_repository.Categories.ToList(), JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = Int32.MaxValue;
        //    return jsonResult;
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        //{

        //    var results = new List<CategoryViewModel>();

        //    if (cat != null && ModelState.IsValid)
        //    {

        //        _repository.Create(cat);
        //        cat.ID = _repository.Categories.First().ID;
        //        results.Add(cat);
        //    }


        //    return Json(results.ToDataSourceResult(request, ModelState));
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        //{
        //    if (cat != null && ModelState.IsValid)
        //    {

        //        _repository.Edit(cat);
        //    }

        //    return Json(ModelState.ToDataSourceResult());
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        //{
        //    if (cat != null)
        //    {
        //        var item = _repository.Get(cat.ID);
        //        _repository.Delete(item);
        //    }


        //    return Json(ModelState.ToDataSourceResult());
        //}

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}