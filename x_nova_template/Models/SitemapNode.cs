using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Routing;
using x_nova_template.Areas.Admin.Controllers;

namespace x_nova_template.Models
{
    public class SitemapNode
    {
        public string Url { get; set; }
        public DateTime? LastModified { get; set; }
        public SitemapFrequency? Frequency { get; set; }
        public double? Priority { get; set; }


        public SitemapNode(Menu page)
        {
            Url = GetPageUrl(page);     
            Priority = null;
            Frequency = null;
            LastModified = null;
        }
        public SitemapNode(string url)
        {
            Url = ConfigurationManager.AppSettings["LocalPath"]+url;
            Priority = null;
            Frequency = null;
            LastModified = null;
        }

        public SitemapNode(RequestContext request, object routeValues)
        {
            Url = GetUrl(request, new RouteValueDictionary(routeValues));
            Priority = null;
            Frequency = null;
            LastModified = null;
        }

        private string GetPageUrl(Menu page) {
            if (page.ParentId == 0)
            {
                return ConfigurationManager.AppSettings["LocalPath"] + "/" + page.Url;
            }
            else {
                return ConfigurationManager.AppSettings["LocalPath"]+"/"
                    + MenuController.GetMenuUrl(page.ParentId)+"/"+page.Url;
            }
        }

        private string GetUrl(RequestContext request, RouteValueDictionary values)
        {

            
            var routes = RouteTable.Routes;
            
            var data = routes.GetVirtualPath(request, values);
            
            if (data == null)
            {
                return null;
            }

            var baseUrl = request.HttpContext.Request.Url;
            
            var relativeUrl = data.VirtualPath;

            return request.HttpContext != null &&
                   (request.HttpContext.Request != null && baseUrl != null)
                       ? new Uri(baseUrl, relativeUrl).AbsoluteUri
                       : null;
        }



    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }
}