namespace x_nova_template.Binders
{
    using x_nova_template.Models;
    using System;
    using System.Web.Mvc;

    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = (Cart) controllerContext.HttpContext.Session["Cart"];
            if (cart != null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session["Cart"] = cart;
            }
            return cart;
        }
    }
}

