using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Filters
{
    public class ControllerFilters
    {

        
    }

    public class DisableController : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                throw new HttpException(filterContext.RouteData.Values["action"] as string);
            }
    }
}