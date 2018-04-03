using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using x_nova_template.Controllers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        //
        // GET: /Admin/Slider/

         ISliderRepository _db;

        public SliderController(ISliderRepository repository)
        {
           _db = repository;

        }

        public ActionResult Index(){
            //dfg
            return View(_db.getAll());
        }
        public ActionResult GetSlider(){
            var list = _db.getAll();
            return PartialView(list);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Portfolio folio,HttpPostedFileBase file){
            if (folio != null)
            {
                _db.Create(folio, file);
                return RedirectToAction("Index");
            }
            else return View();
            
        }
        public JsonResult GetData() {
            var list = _db.getAll().Select(x=>new Portfolio{
            ID=x.ID,Title=x.Title,Description=x.Description,Price=x.Price}).ToArray();
            return Json(new {dataList=list },JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSlide(int id) {
            var item = _db.GetPortfolio(id);
            return Json(new { title = item.Title, desc = item.Description, price = item.Price }, JsonRequestBehavior.AllowGet);
        }
        public void GetFolioSmall(int id)
        {
            Portfolio image = _db.GetPortfolio(id);
            if (image != null)
            {
                new WebImage(image.ImageData).Resize(75,75).Crop(1,1).Write();
            }
           
        }
        public static int GetFirstSlide() {
            ISliderRepository db = new SliderRepository();
            return db.getAll().First().ID;
        }
        
        public void GetSliderImage(int id,int width=0,int height=0)
        {
            Portfolio image = _db.GetPortfolio(id);
            if (image != null)
            {
                if (width != 0||height!=0)
                {
                    new WebImage(image.ImageData)
                              .Resize(width, height,true,true) // 
                              .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                              .Write();
                }
                else {
                    new WebImage(image.ImageData)
                          
                          .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                          .Write();
                }
            }
           
        }
        public ActionResult Edit(int id){
           
                var item = _db.GetPortfolio(id);
                return View(item);            
        }
        [HttpPost]
        public ActionResult Edit(Portfolio folio,HttpPostedFileBase file)
        {
           
            
            if (ModelState.IsValid)
            {

               _db.Edit(folio, file);
                return RedirectToAction("Index");
            }
            else return View();
        }

        public ActionResult Delete(int id) {
            var item = _db.GetPortfolio(id);
            _db.Delete(item);
            return RedirectToAction("Index");
        }
    }
}
