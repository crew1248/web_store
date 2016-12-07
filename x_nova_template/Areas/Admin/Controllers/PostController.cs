using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
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

        public ActionResult Details(int id = 0)
        {
            Post post = _rep.Get(id);
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
            if (ModelState.IsValid && file != null)
            {


                _rep.Create(post, file);
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
        public void GetPostImage(int id, bool isList = true, string type = null)
        {
            var item = _rep.Get(id);

            if (isList)
            {
                new WebImage(item.PreviewPhoto)
                   .Resize(75, 75, false)
                   .Crop(1, 1)
                   .Write();
            }
            else
            {
                if (type == "large")
                {
                    new WebImage(item.PreviewPhoto)
                    .Resize(500, 500, true, true)
                   .Crop(1, 1)
                   .Write();
                }
                else if (type == "small")
                {
                    new WebImage(item.PreviewPhoto)
                    .Resize(100, 100, true, true)
                    .Crop(1, 1)
                    .Write();
                }
                else
                {
                    new WebImage(item.PreviewPhoto)
                       .Resize(300, 300, true, true)
                       .Crop(1, 1)
                       .Write();
                }
            }

        }

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
        public ActionResult Edit(Post post, HttpPostedFileBase file = null, int cpage = 1)
        {
            if (ModelState.IsValid)
            {
                _rep.Edit(post, file);
                TempData["message"] = "Статья отредактирована";
                TempData["type"] = 1;
                return RedirectToAction("Index", new { page = cpage });
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Delete/5
        public string GetRandomName(int c = 0)
        {
            var str = Path.GetRandomFileName();
            str = c == 0 ? 0 + str : c.ToString() + str;
            return str.Replace(".", "");
        }
        public int CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return 0;
            }
            else return Directory.GetFiles(path).Count();

        }
        public ActionResult PostUploadStart(IEnumerable<HttpPostedFileBase> files, string id)
        {

            //return Json(new { type = "error",mess= files.First().ContentLength }, "text/plain", JsonRequestBehavior.AllowGet);
            if (files != null && id != null)
            {

                foreach (var file in files)
                {
                    if (file.ContentLength > 4000000) return Json(new { type = "length", mess = "Размер файла превышает 4 МБ" }, "text/plain", JsonRequestBehavior.AllowGet);

                    string fileName = null;
                    string physicalPath = null;
                    string dirPath = null;

                    dirPath = Server.MapPath("~/Content/Files/Post/" + id);
                    int imagesCount = CheckDirectory(dirPath);
                    physicalPath = Path.Combine(Server.MapPath("~/Content/Files/Post/" + id), GetRandomName(imagesCount) + Path.GetExtension(file.FileName));

                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                    {



                        WebImage formImg = null;
                        byte[] imgBytes = null;
                        if (file.ContentLength > 1000000)
                        {
                            formImg = new WebImage(file.InputStream);
                            imgBytes = formImg.Resize(formImg.Width / 2, formImg.Height / 2).GetBytes();

                        }
                        else imgBytes = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }



                    }
                    else return Json(new { type = "length", mess = "допускаемые расширения файла: jpg,jpeg,png" }, "text/plain", JsonRequestBehavior.AllowGet);

                }

                return Content("");
            }
            return Json(new { type = "error" }, "text/plain", JsonRequestBehavior.AllowGet);
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

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                        Dispose();
                    }
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

                // TODO: Verify user permissions

                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                    Dispose();
                }
            }
            return Json("");
        }
        public ActionResult Delete(int id = 0, int cpage = 1)
        {
            Post post = _rep.Get(id);
            if (post != null)
            {
                ViewBag.page = cpage;
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