using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;


namespace x_nova_template.Exstentions
{
    public static class ConfirmUser

    {

        public static void SendConfirm(string confirmToken,string name,string uemail)
        {
            MailMessage email = new MailMessage();

            email.From = new MailAddress("crew1251@yandex.ru");
            email.To.Add(new MailAddress(uemail));

            email.Subject = "Подтверждение регистрации / OnTrue.ru";
            email.IsBodyHtml = true;
            string link = "http://localhost:3710/Account/ConfirmRegister/?username=" + name + "&confirm=" + confirmToken;
            email.Body = "<p> проследуйте по ссылке для подтверждения регистрации <a href=" + link + ">" + link + "</a></p>";
            email.Body += "<p>Use this link for Account confirm </p>";

            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = true;
                smtp.Host = "smtp.yandex.ru";
                smtp.Port = 25;
                smtp.Credentials = new NetworkCredential("crew1251@yandex.ru", "shamutra");

                smtp.Send(email);
            }


        }

        public static string HashResetParams(string username, string guid)
        {

            byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + guid);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));

            return HashParams;
        }
    }
}