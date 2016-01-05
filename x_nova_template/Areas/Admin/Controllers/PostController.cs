using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        public ActionResult Index(int page=1)
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

        public ActionResult LastNews() {
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
        public ActionResult Create(Post post,HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file!=null)
            {
                post.CreatedAt = DateTime.Now;
                _rep.Create(post, file);
                TempData["message"] = "Статья опубликована";
                TempData["type"] = 1;
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "заполните все поля");
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Edit/5
        public void GetPostImage(int id,bool isList=true,string type=null)
        {
            var item = _rep.Get(id);
            if (item.Preview != null)
            {
                if (isList)
                {
                    new WebImage(item.PreviewPhoto)
                       .Resize(75, 75,false)
                       .Crop(1, 1)
                       .Write();
                }
                else {
                    if (type == "large")
                    {
                        new WebImage(item.PreviewPhoto)
                        .Resize(500,500,true,true)
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
                           .Resize(300,300,true,true)
                           .Crop(1, 1)
                           .Write();
                    }
                }
            }
        }

        public ActionResult Edit(int id = 0,int cpage=1)
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
        public ActionResult Edit(Post post, HttpPostedFileBase file=null,int cpage=1)
        {
            if (ModelState.IsValid)
            {
                _rep.Edit(post,file);
                TempData["message"] = "Статья отредактирована";
                TempData["type"] = 1;
                return RedirectToAction("Index", new { page=cpage});
            }
            return View(post);
        }

        //
        // GET: /Admin/Post/Delete/5

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
            return RedirectToAction("Index", new { page=cpage});
        }

        
    }
}