using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using x_nova_template.Areas.Admin.Controllers;
using x_nova_template.Binders;
using x_nova_template.Extension;
using x_nova_template.Migrations;
using x_nova_template.Models;
using x_nova_template.Service;

namespace x_nova_template
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           

            var migrator = new DbMigrator(new Configuration());
            migrator.Configuration.AutomaticMigrationDataLossAllowed = false;
            migrator.Update();

            //Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("iphone")
            {
                ContextCondition = Context =>
                                Context.Request.Browser["HardwareModel"] == "iPhone"
            });

            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("android")
            {
                ContextCondition = Context =>
                                Context.Request.Browser["PlatformName"] == "Android"
            });

            DisplayModeProvider.Instance.Modes.Insert(2, new DefaultDisplayMode("mobile")
            {
                ContextCondition = Context =>
                                Context.Request.Browser["IsMobile"] == "True"
            });
        }

        protected void Application_BeginRequest()
        {
            
          

        //    if (!Context.Request.IsSecureConnection)
        //        Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            
        }
        protected void Session_Start(object sender, EventArgs e)
        {

            HttpContext.Current.Response.Headers.Remove("X-XNOVA-Version");
            HttpContext.Current.Response.Headers.Add("X-XNOVA-Version", "3.2");
            //        protected void Page_Load(object sender, EventArgs e)
            //{
            //    this.countMe();

            //    DataSet tmpDs = new DataSet();
            //    tmpDs.ReadXml(Server.MapPath("~/counter.xml"));

            //    lblCounter.Text = tmpDs.Tables[0].Rows[0]["hits"].ToString();
            //}

            //private void countMe()
            //{
            //    DataSet tmpDs = new DataSet();
            //    tmpDs.ReadXml(Server.MapPath("~/counter.xml"));

            //    int hits = Int32.Parse(tmpDs.Tables[0].Rows[0]["hits"].ToString());

            //    hits += 1;

            //    tmpDs.Tables[0].Rows[0]["hits"] = hits.ToString();

            //    tmpDs.WriteXml(Server.MapPath("~/counter.xml"));

            //}
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }
        protected void Application_AcquireRequestState()
        {
            // GetYaToken();
        }
        protected void GetYaToken()
        {

            if (HttpContext.Current != null)
            {
                var code = HttpContext.Current.Request.QueryString["code"];

                string error = HttpContext.Current.Request.QueryString["error"];

                string errorDescr = HttpContext.Current.Request.QueryString["error_description"];

                if ((code == null) && (error == null))
                {
                    // Что-то пошло не так
                    GlobalVariables.ErrorText = "Ошибка: отсутствуют QQQ параметры code или error";
                    //.Text = "Ошибка: отсутствуют параметры code или error";
                    return;
                }

                if (error != null)
                {

                    // Раз есть описание ошибки - значит, все равно что-то пошло не так
                    GlobalVariables.ErrorText = "Ошибка: " + error + "описание" + errorDescr;
                    return;
                }

                YaMoney yaNet = new YaMoney();
                // А если все так - получаем токен из временного ключа
                String getTokenResult = yaNet.GetAccessTokenFromTemporaryToken(code);


                if (yaNet.AccessToken != "")
                {
                    // Все ок - сохраняем токен (например, как тут, в сессию) и возвращаемся на сайт
                    Session["token"] = yaNet.AccessToken;
                    GlobalVariables.AccessToken = yaNet.AccessToken;
                    GlobalVariables.ErrorText = code;
                    Response.Redirect("/Checkout/Finished?transactionOk=true");
                }
                else
                {
                    // Получить токен по временному ключу не удалось 
                    GlobalVariables.ErrorText = getTokenResult;
                }
            }
        }

        protected void Application_Error()
        {
            if (Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SiteLiveStatus"]))
            {
                var exception = Server.GetLastError();
                var httpException = exception as HttpException;
                Response.Clear();
                Server.ClearError();
                var routeData = new RouteData();
                routeData.Values["controller"] = "Errors";
                routeData.Values["action"] = "General";
                routeData.Values["exception"] = exception;
                Response.StatusCode = 500;

                if (httpException != null)
                {
                    Response.StatusCode = httpException.GetHttpCode();
                    switch (Response.StatusCode)
                    {
                        case 403:
                            routeData.Values["action"] = "Http403";
                            break;
                        case 404:
                            routeData.Values["action"] = "Http404";
                            break;
                        case 410:
                            routeData.Values["action"] = "Offline";
                            break;

                    }
                }

                IController errorsController = new ErrorsController();
                var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
                errorsController.Execute(rc);
            }
        }
    }
}
