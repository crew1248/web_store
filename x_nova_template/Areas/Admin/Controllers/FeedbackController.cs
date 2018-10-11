using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Areas.Admin.Controllers
{

    public class FeedbackController : Controller
    {
        //
        // GET: /Admin/Feedback/

        IEmailSender _emailSender;

        public FeedbackController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        [HttpPost]
        public JsonResult ValidCaptcha(string Captcha)
        {

            var isValidCaptcha = DataCaptchaController.IsValidCaptchaValue(Captcha);



            if (!isValidCaptcha)
            {
                return Json(Captcha == null);

            }
            return Json(true);
        }

        public ActionResult Send()
        {
            System.Threading.Thread.Sleep(1000);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(Feedback feed)
        {
            //throw new HttpException();
            System.Threading.Thread.Sleep(1500);
            /*var isValidCaptcha = DataCaptchaController.IsValidCaptchaValue(feed.Captcha);

            
            if (!isValidCaptcha)
            {
                throw new HttpException();
            }*/

            if (ModelState.IsValid)
            {
                _emailSender.SendMail(feed.Name, feed.Text, "Письмо с сайта!", feed.Email);

                if (Request.IsAjaxRequest())
                {

                    return PartialView("Feedbacksend");
                }
                return Redirect("/");
            }
            throw new HttpException();
        }


    }
}
