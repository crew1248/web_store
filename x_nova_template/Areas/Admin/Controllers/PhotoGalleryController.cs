﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;
using x_nova_template.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class PhotoGalleryController : Controller
    {
        //
        // GET: /Admin/PhotoGallery/

        IPhotoGallery _gRepo;

        public PhotoGalleryController(IPhotoGallery gRepo) {
            _gRepo = gRepo;
        }

        public ActionResult Index(int page=1)
        {
            int pageSize = 15;

            PhotoGalleryViewModel vm = new PhotoGalleryViewModel
            {
                Galleries = _gRepo.GalAll()
                    .OrderByDescending(x => x.ID)
                    .ToList()                    
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _gRepo.Galleries.Count(),
                    ItemsPerPage = pageSize,
                    CurrentPage = page
                },
                Images=_gRepo.Images.ToList()
            };
            
            return View(vm);
           
        }

        public ActionResult GetFirstHalfPortfolio() {
            var items = _gRepo.GalAll().OrderBy(x=>x.ID).Take(_gRepo.GalAll().Count() / 2);
            return PartialView(items);
        }
        public ActionResult GetSecondHalfPortfolio() {
            var items = _gRepo.GalAll().OrderBy(x => x.ID).Skip(_gRepo.GalAll().Count() / 2);
            return PartialView(items);
        }
        public FileContentResult GetPhoto(int id) {
            var item = _gRepo.GetImage(id);
            if (item != null)
            {

                return File(item.ImageData, item.ImageMimeType);
            }
            else return null;
        }

        public FileContentResult GetGalleryPhoto(int id)
        {
            var item = _gRepo.GetGallery(id);
            if (item != null)
            {

                return File(item.GalleryData, item.GalleryMimeType);
            }
            else return null;
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Gallery gallery,HttpPostedFileBase file,Image image=null,int galId=0) {

            if (gallery == null)
            {
                if (ModelState.IsValid)
                {
                    _gRepo.Create(null,file,galId);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TempData["message"] = "Галлерея - " + gallery.GalleryTitle + " создана";
                    TempData["type"] = 1;
                    _gRepo.Create(gallery, file, 0);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        { 
        
            var item = _gRepo.GetGallery(id);
          
            return View(item);

        }
        public JsonResult Folder(int id) {
            List<int> arr = _gRepo.GetGallery(id).Images.Select(x=>x.ID).ToList();
            return Json(new {list=arr }, JsonRequestBehavior.AllowGet);
        }
        public FileContentResult GetImage(int id)
        {
            Image image = _gRepo.GetImage(id);
            if (image != null)
            {
                return File(image.ImageData, image.ImageMimeType);
            }
            else { return null; }
        }
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> files,int galId) {

            if (files != null) {
                foreach (var file in files) {                     
                    _gRepo.Create(null, file, galId);
                }
            }

            return Content("");
        }

        public ActionResult UploadStart(int galId)
        { 
            ViewBag.GalId = galId;
            return PartialView();
        }

        public ActionResult DeleteImg(int id) {
            if (id != 0)
            {
                var item = _gRepo.GetImage(id);
                _gRepo.Delete(null, item);
                return Json(new { imgId = id });
            }
            return Content("");
        }

        public ActionResult Delete(int id)
        {
            var gal  = _gRepo.GetGallery(id);
            if (gal.Images.Count() != 0)
            {
                foreach (var item in gal.Images)
                {
                    _gRepo.Delete(null, item);
                }
            }
            TempData["message"] = "Галлерея - " + gal.GalleryTitle + " Удалена";
            TempData["type"] = 1;
            _gRepo.Delete(gal);
            return RedirectToAction("Index");


        }

        public ActionResult Edit(int id,int page) {
            ViewBag.Page = page;
            var item = _gRepo.GetGallery(id);
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(Gallery gal,HttpPostedFileBase file=null)
        {
            if(ModelState.IsValid){
                TempData["message"] = "Галлерея - " + gal.GalleryTitle + " отредактированна";
                TempData["type"] = 1;
                _gRepo.Edit(gal,null,file);
            }
            return RedirectToAction("Index");
        }
    }
}