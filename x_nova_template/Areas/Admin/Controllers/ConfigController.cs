using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;

namespace x_nova_template.Areas.Admin.Controllers
{
    
    public class ConfigController : Controller
    {
        private const string SitemapsNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        //
        // GET: /Admin/Config/
        private IConfigRepository _rep;
        private IMenuRepository _menu;
        public ConfigController(IConfigRepository rep,IMenuRepository menu)
        {
            _menu=menu;
            _rep = rep;
        }
        public ActionResult GetPartials() {
            return PartialView();
        }
        public ActionResult Change()
        {
            var item = _rep.Configs.First();
            return View(item);
        }
        [HttpPost]
        public ActionResult Abandon()
        {
            Session.Abandon();
            Session.Clear();
            return Json("");
        }
        [HttpPost]
        public ActionResult Change(Config conf)
        {
            if (ModelState.IsValid)
            {
                _rep.Edit(conf);
                TempData["message"] = "настройки сохранены";
                TempData["type"] = 1;
            }
            return Redirect("/Admin");
        }

        public ActionResult OfflineMess() {
            var item = _rep.Configs.First();
            return PartialView(item);
        }
        [NonAction]
        public string GetSitemapXml() {
            XElement root;
            XNamespace xmlns = SitemapsNamespace;

            var nodes = GetSitemapNodes();

            root = new XElement(xmlns + "urlset");                    

            foreach (var node in nodes) { 
                root.Add(
                new XElement(xmlns+"url",
                    new XElement(xmlns+"loc",Uri.EscapeUriString(node.Url)),
                     node.Priority == null ? null : new XElement(xmlns + "priority", node.Priority.Value.ToString("F1", 
                        CultureInfo.InvariantCulture)),
                     node.LastModified == null ? null : new XElement(xmlns + "lastmod",node.LastModified.Value.ToLocalTime()
                        .ToString("yyyy-MM-ddTHH:mm:sszzz")),
                     node.Frequency == null ? null :new XElement(xmlns + "changefreq", node.Frequency.Value.ToString()
                        .ToLowerInvariant())
                    ));
            }

            using (MemoryStream ms = new MemoryStream()) {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8)) {
                    root.Save(sw);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }

            
        }

        [NonAction]
        public IEnumerable<SitemapNode> GetSitemapNodes() {
            List<SitemapNode> nodes = new List<SitemapNode>();
            var localPath = ConfigurationManager.AppSettings["LocalPath"];
            
            nodes.Add(new SitemapNode("/Home")
            {
                Frequency = SitemapFrequency.Always,
                Priority = 1
            });

            var pages = _menu.Menues.ToList();

            foreach(var page in pages.Where(x=>x.Url!="Home"&&x.MenuSection<=1)){
                
                nodes.Add(new SitemapNode(page){
                    Frequency = SitemapFrequency.Yearly,
                    Priority= 0.8,
                    LastModified  = page.LastModifiedDate 
                });
            }
            return nodes;
                
            
        }

        [HttpGet]
        [OutputCache(Duration = 24 * 60 * 60, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult SitemapXml() {
            Trace.WriteLine("sitemap.xml was requested. User agent: " + Request.Headers.Get("User-Agent"));

            var sitemap = GetSitemapXml();

            return Content(sitemap, "application/xml", Encoding.UTF8);
        }

        [OutputCache(Duration=60*60*24,Location=System.Web.UI.OutputCacheLocation.Any)]
        public FileContentResult RobotsText() {
            var str = new StringBuilder("User-agent:*" + Environment.NewLine);

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SiteLiveStatus"]))
            {
                str.Append("Disallow: /Account" + Environment.NewLine);
                str.Append("Disallow: /Signalr" + Environment.NewLine);
                str.Append("Disallow: /LiveChat" + Environment.NewLine);
                str.Append("Disallow: /Consultant" + Environment.NewLine);
                str.Append("Disallow: /Error" + Environment.NewLine);

                str.Append("Sitemap: " + ConfigurationManager.AppSettings["LocalPath"] + "/sitemap.xml" + Environment.NewLine);
            }
            else {
                str.Append("Disallow: /"+Environment.NewLine);
            }
            return File(Encoding.UTF8.GetBytes(str.ToString()),"text/plain");
        }

        public static Config SiteOptions() {
            IConfigRepository conf = new ConfigRepository();
            var item = conf.Options();
            return item;
        }


       
    }
}
