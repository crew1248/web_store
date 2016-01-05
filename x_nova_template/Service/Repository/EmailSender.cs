using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using x_nova_template.Service.Interface;
using System.Configuration;

namespace x_nova_template.Service.Repository
{
    public class EmailSender:IEmailSender
    {
        public void SendMail(string name, string message, string to) {
            StringBuilder body = new StringBuilder()
                    .AppendLine("Новое сообщение было отправлено!")
                    .AppendLine("---")
                    .AppendLine("Имя : " + name)

                    .AppendLine("Сообщение : " + message)

                    .AppendLine("Email : " + to)
                    .AppendLine("---");

            var mess = new MailMessage(
                ConfigurationManager.AppSettings["mailAccount"].ToString(),
                to,
                "Сообщение с сайта " + ConfigurationManager.AppSettings["SiteName"].ToString(),
                body.ToString());
            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Host = ConfigurationManager.AppSettings["mailHost"].ToString();
                smtp.Port = 25;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"].ToString(), ConfigurationManager.AppSettings["mailPassword"].ToString());

                smtp.Send(mess);
            }
        }
    }
}