using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.ViewModel;

namespace x_nova_template.Service.Interface
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, CheckoutViewModel vm);
    }
}