using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.Service.Interface
{
    public interface IEmailSender
    {
        void SendMail(string name,string message,string title,string to);
    }
}