using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using x_nova_template.Controllers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{

    public class PostController : BaseController
    {
        IPostRepository _rep;

        public PostController(IPostRepository rep)
        {
            _rep = rep;
        }
        //
        // GET: /Admin/Post/

        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;
            PostViewModel vm = new PostViewModel
            {
                Posts = _rep.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _rep.Posts.Count(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize

                }
            };
            return View(vm);
        }
        [OutputCache(Duration = 60)]
        public ActionResult LastNews()
        {
            PostViewModel vm = new PostViewModel
            {
                Posts = _rep.Posts
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
                    .Take(3)
            };
            return PartialView(vm);
        }
        //
        // GET: /Admin/Post/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Post post = _rep.Get((int)id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Post/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Post post, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {


                _rep.Create(post);


                TempData["message"] = "Статья опубликована";
                TempData["type"] = 1;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "заполните все поля");
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Edit/5              

        public ActionResult Edit(int id = 0, int cpage = 1)
        {
            Post post = _rep.Get(id);
            ViewBag.page = cpage;
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Admin/Post/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Post post, int cpage = 1)
        {

            if (ModelState.IsValid)
            {

                _rep.Edit(post);

                TempData["message"] = "Статья отредактирована";
                TempData["type"] = 1;
                return RedirectToAction("Index", new { page = cpage });
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Delete/5

        public ActionResult PostUploadStart(IEnumerable<HttpPostedFileBase> files, int id, bool isWM = false)
        {

            //return Json(new { type = "error",mess= files.First().ContentLength }, "text/plain", JsonRequestBehavior.AllowGet);
            if (files != null && id != 0)
            {

                foreach (var file in files)
                {

                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                    {
                        _rep.SavePhoto(file, id, isWM);

                    }
                    else return Json(new { type = "length", mess = "допускаемые расширения файла: jpg,jpeg,png" }, "text/plain", JsonRequestBehavior.AllowGet);

                }
                //return Json(new { type = "length", mess = "допускаемые расширения файла: jpg,jpeg,png" }, "text/plain", JsonRequestBehavior.AllowGet);
                return Json(new { pimgid = id });
            }
            return Json(new { type = "error" }, "text/plain", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPostImages(int id)
        {
            var prod = _rep.Get(id);
            //todo обновление списка файлов в статьях
            return PartialView(prod);
        }

        public ActionResult PostRemoveStart(string[] fileNames, string id)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Files/Post/" + id), fileName);

                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        [HttpPost]
        public JsonResult RemoveImage(string name, int id)
        {
            //var fileName = Path.GetFileName(name);
            if (name != null)
            {
                var physicalPath = Path.Combine(Server.MapPath("~/Content/Files/Post/" + id), name);
                var physicalPaths = Path.Combine(Server.MapPath("~/Content/Files/Post/" + id + "/200x150"), name);
                _rep.PhotoDel(physicalPath);
                _rep.PhotoDel(physicalPaths);
            }
            return Json("");
        }
        public ActionResult Delete(int id = 0, int cpage = 1)
        {
            Post post = _rep.Get(id);
            if (post != null)
            {
                ViewBag.page = cpage;
                var physicalPath = Server.MapPath("~/Content/Files/Post/" + id);
                _rep.RemoveDir(physicalPath);
                _rep.Delete(post);

                TempData["message"] = "Статья удалена";
                TempData["type"] = 1;
            }
            return RedirectToAction("Index", new { page = cpage });
        }

        //
        // POST: /Admin/Post/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int cpage = 1)
        {
            Post post = _rep.Get(id);
            _rep.Delete(post);
            TempData["message"] = "Статья удалена";
            TempData["type"] = 1;
            return RedirectToAction("Index", new { page = cpage });
        }


    }
}