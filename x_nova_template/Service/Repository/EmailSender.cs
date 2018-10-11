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
    public class EmailSender : IEmailSender
    {
        public void SendMail(string name, string message, string title, string to)
        {
            var username = ConfigurationManager.AppSettings["mailUserName"].ToString();
            var pass = ConfigurationManager.AppSettings["mailPassword"].ToString();
            StringBuilder builder = new StringBuilder()
                .AppendLine("Новое сообщение было отправлено!")
                .AppendLine("---")
                .AppendLine("<b>Имя : </b>" + name)
                .AppendLine("<b>Сообщение : </b>" + message)
                .AppendLine("<b>Email : </b>" + to)
                .AppendLine("---");
            MailMessage message2 = new MailMessage(
                ConfigurationManager.AppSettings["mailAddress"].ToString(),
                ConfigurationManager.AppSettings["mailAddress"].ToString(),
                title,
                builder.ToString())
                {
                    IsBodyHtml = true
                };
            using (SmtpClient client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = ConfigurationManager.AppSettings["mailHost"].ToString();
                client.Credentials = new NetworkCredential(username, pass);
                client.Port = 0x19;
                client.Send(message2);
            }
        }
    }
}