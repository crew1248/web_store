using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.ViewModel;
using x_nova_template.Models;
using x_nova_template.Service.Interface;


namespace x_nova_template.Areas.Admin.Controllers
{
    public class StaticSectionController : Controller
    {
        //
        // GET: /Admin/StaticSection/
        IStaticSectionRepository _rep;

        public StaticSectionController(IStaticSectionRepository rep)
        {
            _rep = rep;
        }

        public ActionResult Index(int page = 1)
        {
            int pageSize = 15;

            SectionsViewModel vm = new SectionsViewModel
            {
                StaticSections = _rep.StaticSections
                  .OrderBy(x => x.ID)
                  .ToList()
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    TotalItems = _rep.StaticSections.Count(),
                    CurrentPage = page,
                    ItemsPerPage = pageSize

                }
            };
            return View(vm);

        }
        public JsonResult GetSections()
        {
            var items = _rep.StaticSections
                .Select(x => new
                {
                    SectionType = x.SectionType,
                    Content = x.Content
                }).ToArray();
            return Json(new { sections = items }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id, int cpage = 1)
        {
            var item = _rep.Get(id);
            _rep.Delete(item);
            TempData["message"] = "Статический блок удален";
            TempData["type"] = 1;
            return RedirectToAction("Index", new { page = cpage });
        }
        public ActionResult Edit(int id, int cpage = 1)
        {
            ViewBag.page = cpage;
            return View(_rep.Get(id));
        }
        [HttpPost]
        public ActionResult Edit(StaticSection model, int cpage = 1)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = "Статический блок изменен";
                TempData["type"] = 1;
                _rep.Edit(model);
                return RedirectToAction("Index", new { page = cpage });
            }
            else return View(model);
        }

        public ActionResult GetSection(SectionType? type)
        {
            switch (type)
            {
                case SectionType.TopQuote:
                    return PartialView(_rep.GetSection(1));
                case SectionType.BottomQuote:
                    return PartialView(_rep.GetSection(2));
                case SectionType.Phone:
                    return PartialView(_rep.GetSection(3));
                case SectionType.Counter:
                    return PartialView(_rep.GetSection(4));
                case SectionType.FirstSection:
                    return PartialView(_rep.GetSection(5));
                case SectionType.SecondSection:
                    return PartialView(_rep.GetSection(5));
                default:
                    return Content("");
            }
        }


        public enum SectionType
        {
            TopQuote,
            BottomQuote,
            Phone,
            FirstSection,
            SecondSection,
            Counter
        }
    }
}
