using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;
using Kendo.Mvc.Extensions;
using x_nova_template.Controllers;
using Newtonsoft.Json;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /Category/

        public int PageSize = 4;
        ICategoryRepository _repository;

        public CategoryController(ICategoryRepository repository)
        {
            _repository = repository;

        }

        public ActionResult Index() {
            var types = new List<SelectListItem> {
                new SelectListItem {Text="игрушка",Value="игрушка"},
                new SelectListItem {Text="пресс-форма",Value="пресс-форма"}
            };
            ViewBag.Types = types;
            return View();
        }

        public ActionResult CatList(int page=1)
        {
            
            return View(_repository.Categories
                .OrderBy(x=>x.ID)
                .Skip((page-1)*PageSize)
                .Take(PageSize));
        }
        public PartialViewResult CatsMenu() {
            return PartialView(_repository.Categories.OrderBy(x=>x.Sortindex).ToList());
        }
        public PartialViewResult CatsMenu2()
        {
            return PartialView(_repository.Categories.Where(x => x.CatType == "игрушка").OrderBy(x => x.Sequance).ToList());
        }
        public ActionResult Create(int id) {
            var result = _repository.Categories.FirstOrDefault(x => x.ID == id);

            return View(result);
                
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
        public ActionResult Filter_Categories() {
            var items = _repository.Categories.Select(
                    x => new CategoryViewModel
                    {
                        ID = x.ID,
                        CategoryName = x.CategoryName
                    }
                );
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _repository.Categories.ToDataSourceResult(request, o => new CategoryViewModel
            {
                CatDescription = o.CatDescription,
                CatType=o.CatType,
                CategoryName = o.CategoryName,
                Sortindex = o.Sortindex,
                ID=o.ID

            });
            return Json(result);
        }
        public JsonResult GetCategories()
        {
            var jsonResult = Json(_repository.Categories.ToList(), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {

            var results = new List<CategoryViewModel>();

            if (cat != null && ModelState.IsValid)
            {

                _repository.Create(cat);
                //cat.ID = _repository.Categories.First().ID;
                results.Add(cat);
            }
             

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {
            if (cat != null && ModelState.IsValid)
            {
              
                    _repository.Edit(cat);                
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, CategoryViewModel cat)
        {
            if (cat != null)
            {
                var item = _repository.Get(cat.ID);
                _repository.Delete(item);
            }


            return Json(ModelState.ToDataSourceResult());
        }

    }
}
