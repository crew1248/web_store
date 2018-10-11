using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        IMenuRepository _repository;

        public MenuController(IMenuRepository repository)
        {
            _repository = repository;
        }

        public ViewResult MenuList(int type = 0, int page = 1)
        {
            int pageSize = 25;
            MenuViewModel vm = new MenuViewModel
            {
                Menues = _repository.Menues
                    .Where(x => x.ParentId == 0 && x.MenuSection == (int)type)
                    .OrderBy(x => x.SortOrder)
                    .ToList()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _repository.Menues.Where(x => x.ParentId == 0 && x.MenuSection == (int)type).Count(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize

                },
                ctype = type
            };
            return View(vm);

        }


        public ActionResult SS(int? menuSection)
        {
            if (menuSection == 1)
            {
                return PartialView(_repository.Menues.Where(x => x.MenuSection == 1).ToList());
            }
            else if (menuSection == 2)
            {
                ViewBag.IsFoot = true;
                return PartialView(_repository.Menues.Where(x => x.MenuSection == 0).ToList());
            }
            else
            {
                return PartialView(_repository.Menues.Where(x => x.MenuSection == 0).ToList());
            }
        }

        [HttpGet]
        public ActionResult Create(int id = 0, int type = 0)
        {

            /*var menu = new Menu();*/

            if (id != 0)
            {

                ViewBag.ParentId = id;
                ViewBag.MenuSection = _repository.Get(id).MenuSection;
            }
            else
            {
                ViewBag.ParentId = 0;
                ViewBag.type = type;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Menu menu)
        {
            if (_repository.Menues.Any(x => x.Url == menu.Url && x.MenuSection == menu.MenuSection))
            {
                //ModelState.AddModelError("", "Раздел с таким именем-URL уже существует");
                TempData["message"] = "Раздел с таким именем-URL уже существует";
                TempData["type"] = 4;
                return View();
            }
            if (ModelState.IsValid)
            {
                _repository.Create(menu);

                HttpRuntime.UnloadAppDomain();
                TempData["message"] = "Раздел " + menu.Text + " создан";
                TempData["type"] = 1;
                if (menu.ParentId == 0)
                {
                    return RedirectToAction("MenuList", new { type = menu.MenuSection });
                }

                return RedirectToAction("Details", new { id = menu.ParentId });

            }
            if (menu.ParentId != 0)
            {

                ViewBag.ParentId = menu.ParentId;
                ViewBag.type = _repository.Get(menu.ParentId).MenuSection;
            }
            else
            {
                ViewBag.ParentId = 0;
            }
            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return View(menu);
        }
        [HttpPost]
        public JsonResult EditSort(string jsonData)
        {
            var result = JsonConvert.DeserializeObject<List<SortViewModel>>(jsonData);
            foreach (var x in result)
            {
                _repository.UpdateSort(Int32.Parse(x.id), Int32.Parse(x.sort));
            }
            return base.Json("");
        }
        public ActionResult Details(int id, int page = 1)
        {
            var item = _repository.Get(id);

            if (item != null)
            {
                ViewBag.page = page;
                ViewBag.type = item.MenuSection;
                return View(item);
            }

            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return RedirectToAction("MenuList", new { type = item.MenuSection, page = page });
        }

        public ActionResult ChildMenus(int id)
        {
            if (id != 0)
            {
                var objs = _repository.Menues.Where(x => x.ParentId == id).OrderBy(x => x.SortOrder).ToList();
                return PartialView(objs);
            }
            return Content("");
        }

        public ActionResult Edit(int id, int page = 1)
        {
            if (id != 0)
            {

                var item = _repository.Get(id);
                ViewBag.type = item.MenuSection;
                ViewBag.page = page;
                ViewBag.sort = item.SortOrder;
                return View(item);
            }
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Menu menu, int page = 1)
        {
            if (ModelState.IsValid)
            {
                _repository.Edit(menu);
                //HttpRuntime.UnloadAppDomain();
                TempData["message"] = menu.ParentId == 0 ? "Раздел " + menu.Text + " отредактирован" : "Подраздел " + menu.Text + " отредактирован";
                TempData["type"] = 1;
                if (menu.ParentId == 0) return RedirectToAction("MenuList", new { type = menu.MenuSection, page = page });
                else return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }
            TempData["message"] = "что-то пошло не так";
            TempData["type"] = 4;
            return View(menu);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id, int page = 1)
        {
            var item = _repository.Get(id);
            ViewBag.page = page;
            return View(item);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Menu menu, int page = 1)
        {

            if (_repository.Menues.Any(x => x.ParentId == menu.Id))
            {
                TempData["message"] = "Удалите сначала все подразделы раздела " + menu.Text;
                TempData["type"] = 4;
                //ModelState.AddModelError("", "Удалите сначала все подразделы меню");
                return RedirectToAction("Details", new { id = menu.Id, page = page });
            }
            var item = _repository.Get(menu.Id);
            if (item == null)
            {
                TempData["message"] = "что-то пошло не так";
                TempData["type"] = 4;
                return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }
            if (item.ParentId == 0)
            {
                TempData["message"] = item.ParentId == 0 ? "Раздел " + item.Text + " удален" : "Подраздел " + item.Text + " удален";
                TempData["type"] = 1;
                _repository.Delete(item);
                return RedirectToAction("MenuList", new { type = menu.MenuSection, page = page });
            }
            else
            {
                TempData["message"] = item.ParentId == 0 ? "Раздел " + item.Text + " удален" : "Подраздел " + item.Text + " удален";
                TempData["type"] = 1;
                _repository.Delete(item);

                return RedirectToAction("Details", new { id = menu.ParentId, page = page });
            }


        }


        #region Ext

        public static string GetMenuName(int? id)
        {
            int newId = id ?? 0;

            IMenuRepository service = new MenuRepository();
            if (newId != 0)
            {
                var item = service.Get(newId);
                return item.Text;
            }
            return "";

        }

        public static string GetMenuUrl(int? id)
        {
            int newId = id ?? 0;

            IMenuRepository service = new MenuRepository();
            if (newId != 0)
            {
                var item = service.Get(newId);
                return item.Url;
            }
            return "";

        }
        public enum NotificationMessage
        {
            Create,
            Edit,
            Delete,
            Error
        }
        #endregion
    }
}
