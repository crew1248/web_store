using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Helpers;
using x_nova_template.Controllers;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using x_nova_template.Service.Interface;
using x_nova_template.Models;
using x_nova_template.Areas.Admin.Controllers;


namespace Posbank.Controllers
{
    public class WidgetController : BaseController
    {
    
         IPhotoGallery _grep;
         IProductRepository _prep;

         public WidgetController(IPhotoGallery gRepo, IProductRepository pRepository)
         {
            _grep = gRepo;
            _prep = pRepository;
        }
        
        public ActionResult Inscreaser(int type = 0, string folder = null, int id = 0, string selector = null,string jsonData=null)
        {
            ViewBag.result = 0;
            ViewBag.el = 0;
            if (type == 3&&folder!=null) {
                var path = Server.MapPath("~" + folder);

                var dir = new DirectoryInfo(path);
                var result = dir.GetFiles().Select(x => string.Concat(folder, x.Name)).ToList();
                var json = JsonConvert.SerializeObject(result);
                
                //var formated =json;
                //var path1 = Server.MapPath("~/data.json");
                //using (StreamWriter file = new StreamWriter(path1))
                //{
                //    JsonSerializer serializer = new JsonSerializer();
                //    serializer.Serialize(file, formated);
                //}
                
                ViewBag.result = json;
                var traceSource = new TraceSource("Tracing");
                traceSource.TraceEvent(TraceEventType.Warning, 0, HttpUtility.HtmlEncode(json));
            }
            else if (type == 2 && id != 0) {

                var result = _grep.GetGallery(id).Images.Select(x => x.ID).ToList();
                var json = JsonConvert.SerializeObject(result);
                ViewBag.result = json;
                var traceSource = new TraceSource("Tracing");
                traceSource.TraceEvent(TraceEventType.Warning, 0, HttpUtility.HtmlEncode(json));
            }
            else if (type == 1 && id != 0)
            {
                var item = _prep.Get(id);
                var cart = Session["Cart"] as Cart;
                var isAdded = cart != null ? cart.Lines.Select(x => x.Product.ID).Contains(id) : false;

                JObject result = new JObject
                {
                    {"id",id},
                    {"cat",ProductController.GetCatName(item.CategoryID)},
                    {"name",item.ProductName},
                    {"added", isAdded},
                    {"desc",item.Description},
                    {"si",new JArray{
                        item.ProdImages.Select(x => x.ID).ToArray()
                    }},
                    {"price",item.Price}
                };

                var json = result.ToString();
                ViewBag.result = json;
                var traceSource = new TraceSource("Tracing");
                traceSource.TraceEvent(TraceEventType.Warning, 0, json);
            }
            else {
                if (jsonData != null)
                {
                    var obj = JObject.Parse(jsonData);
                    ViewBag.el = obj["el"];
                    ViewBag.result = obj["pack"];
                }
            }
            ViewBag.type = type;
            ViewBag.folder = folder;
            ViewBag.selector = selector;
            
            return PartialView();
        }
        public void LoadMainPic(string path)
        {
            if (path != null && new WebImage(path).GetBytes() != null)
            {
                new WebImage(path)
                    .Crop(1, 1)
                    .Resize(750, 500, true, true)

                    .Write();
            }
        }

        public void LoadSmallPic(string path)
        {
            if (path != null && new WebImage(path).GetBytes()!=null)
            {
                new WebImage(path)
                    .Resize(100, 100)
                    .Crop(1, 1)
                    .Write();
            }
        }
    }
}
