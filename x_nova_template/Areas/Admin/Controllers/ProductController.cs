using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;
using Kendo.Mvc.Extensions;
using x_nova_template.Service.Repository;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Web.Helpers;
using x_nova_template.Binders;
using x_nova_template.Controllers;

using System.Xml;
using System.Diagnostics;
using System.Data;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace x_nova_template.Areas.Admin.Controllers
{
    
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        public int PageSize = 8;

        IProductRepository _pRepository;
        ICategoryRepository _catRep;
        ICategoryRepository _cRep;
        IConfigRepository _conf;

        protected override void OnActionExecuting(ActionExecutingContext filerContext)
        {

            ViewBag.Str = "SSS";
            base.OnActionExecuting(filerContext);
        }
        public ProductController(IProductRepository pRepository, IConfigRepository conf, ICategoryRepository cRep,ICategoryRepository catRep)
        {
            _cRep = cRep;
            _pRepository = pRepository;
            _conf = conf;
            _catRep = catRep;
        }

        public ActionResult Details(int id)
        {
            var item = _pRepository.Products.SingleOrDefault(x => x.ID == id);
            return View(item);
        }
        public ActionResult ToyDetails(int id)
        {
            var item = _pRepository.Products.SingleOrDefault(x => x.ID == id);
            return View(item);
        }
        public ViewResult ProdList(int catId=0, int page = 1, string credentialToken = null, string resetToken = null)
        {
            ViewBag.Test = Session["test"];
            if (_conf.Options().SelectedIsOnlineID)
            {
                throw new HttpException(410, "Offline");
            }
            ViewBag.Token = credentialToken;
            ViewBag.ResetToken = resetToken;
            ViewBag.CatID = catId == 0 && _pRepository.Products.Count()>0 ? _catRep.Get().Where(x => x.Products.Count() > 0).First().ID : catId;
            ProductListViewModel vm = new ProductListViewModel
            {
                Products = _pRepository.Products
                    .Where(x => x.CategoryID == catId)
                    .OrderBy(x => x.ID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    Service = "Product",
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _pRepository.Products.Where(x => x.CategoryID == catId).Count(),
                    CatId = catId


                },

            };


            return View(vm);
        }

        public ActionResult ProdListPartial(string jsonData)
        {
            JObject obj2 = JObject.Parse(jsonData);
            int id = obj2.SelectToken("catId").Value<int>();
            int num = obj2.SelectToken("page").Value<int>();
            ProductListViewModel model2 = new ProductListViewModel
            {
                Products = (from x in _pRepository.Products
                            where x.CategoryID == id
                            orderby x.ID descending 
                            select x).Skip<Product>(((num - 1) * this.PageSize)).Take<Product>(this.PageSize)
            };
            PagingInfo info = new PagingInfo
            {
                Service = "Product",
                CurrentPage = num,
                ItemsPerPage = this.PageSize,
                TotalItems = (from x in this._pRepository.Products
                              where x.CategoryID == id
                              select x).Count<Product>(),
                CatId = id
            };
            model2.PagingInfo = info;
            ProductListViewModel model = model2;
            return PartialView(model);
        }
        public ViewResult ProdListToy(int catId, int page = 1, string credentialToken = null, string resetToken = null)
        {
            if (_conf.Options().SelectedIsOnlineID)
            {
                throw new HttpException(410, "Offline");
            }
            ViewBag.Token = credentialToken;
            ViewBag.ResetToken = resetToken;

            ProductListViewModel vm = new ProductListViewModel
            {
                Products = _cRep.Categories.Where(x => x.ID == catId && x.CatType == "игрушка").SingleOrDefault().Products
                    .OrderBy(x => x.ID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    Service = "Product",
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _cRep.Categories.Where(x => x.ID == catId && x.CatType == "игрушка").SingleOrDefault().Products.Count(),
                    CatId = catId


                },

            };


            return View(vm);
        }
        //public PartialViewResult RndProds(int catId,int pid) {
        //    var list = _pRepository.Products.Where(x => x.CategoryID == catId && x.ID != pid).ToArray();
        //    var rnd = new Random();
        //    var result = rnd.Next(list.Count());
        //    var rndlist = new List<Product>();
        //    for(var i=0;)


        //    return PartialView(result);
        //}
        public ActionResult ExcelImport() {

            return View();
        }
        public ActionResult ExcelImport(HttpPostedFileBase file) {
          
            return View();
        }

        public static bool ProdIsAdded(int id) {
            Cart cart = (Cart)System.Web.HttpContext.Current.Session["Cart"];
            bool isAdded=false;
            if (cart != null)
            {
                isAdded = cart.Lines.Select(x => x.Product.ID).Contains(id);
            }
            return isAdded;
        }
        public JsonResult ItemInfo(int id)
        {
           // Thread.Sleep(500000);
            var item = _pRepository.Get(id);
            var isAdded = GetCart().Lines.Select(x => x.Product.ID).Contains(id);
            return Json(new
            {
                cat = GetCatName(item.CategoryID),
                name = item.ProductName,
                added = isAdded,
                desc = item.Description,
                si = item.ProdImages.Select(x => x.ID).ToArray(),
                price = item.Price
            }, JsonRequestBehavior.AllowGet);
        }
      
        public JsonResult All_Sl_Images()
        {
            var cats = _cRep.GetForSlider().ToList();
            var prodList = _pRepository.Get();
            List<Product> prods = new List<Product>();

            foreach (var cat in cats.Where(x => x.Products.Count() > 0))
            {
                foreach (var pro in cat.Products.Take(2))
                {
                    prods.Add(pro);
                }

            }
            var idlist = prods.Select(x => new { pid = x.ID, title = x.ProductName }).ToArray();
            return Json(new { pids = idlist }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProdsToSlider()
        {
            var cats = _cRep.GetForSlider().ToList();
            var prodList = _pRepository.Get();
            List<Product> prods = new List<Product>();

            foreach (var cat in cats.Where(x => x.Products.Count() > 0))
            {
                foreach (var pro in cat.Products.Take(2))
                {
                    prods.Add(pro);
                }

            }
            return PartialView(prods.ToList());
        }

        public ActionResult Index()
        { 
           
            ViewBag.Cats = new SelectList(_cRep.Categories, "ID", "CategoryName");
            ViewData["Catss"] = _cRep.Categories
                        .Select(e => new
                        {
                            ID = e.ID,
                            CategoryName = e.CategoryName

                        });
            return View();
        }


        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            DataSourceResult result = _pRepository.Products.ToDataSourceResult(request, o => new ProductViewModel
            {
                ProductName = o.ProductName,
                Description = o.Description,
                Price = o.Price,
                Season = o.Season,
                Size = o.Size,
                ProductType = o.ProductType,
                CategoryName = _cRep.Categories.First(x => x.ID == o.CategoryID).CategoryName,
                ID = o.ID,
                CategoryID = o.CategoryID


            });
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, ProductViewModel prod)
        {

            var results = new List<ProductViewModel>();

            if (prod != null && ModelState.IsValid)
            {

                _pRepository.Create(null, prod);
                prod.ID = _pRepository.Products.OrderByDescending(x => x.ID).First().ID;
                results.Add(prod);
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null && ModelState.IsValid)
            {
                var target = _pRepository.Products.Single(x => x.ID == product.ID);
                if (target != null)
                {

                    target.ID = product.ID;
                    target.Price = product.Price;
                    target.ProductName = product.ProductName;
                    target.Description = product.Description;
                    target.Season = product.Season;
                    target.ProductType = product.ProductType;
                    target.Size = product.Size;

                    _pRepository.Edit(target);
                }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (product != null)
            {
                var item = _pRepository.Get(product.ID);
                _pRepository.Delete(item);
            }


            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult UploadProdImages()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadProdImages(IEnumerable<HttpPostedFileBase> photos, int pid)
        {
            List<int> list = new List<int>();
            int item = 0;
            if (photos != null)
            {
                foreach (var file in photos)
                {
                    item = _pRepository.SavePhoto(file, pid);
                    //list.Add(item);
                }
            }

            return Json(new { pimgid = item });
        }

        public ActionResult GetProdImages(int pid)
        {
            var prod = _pRepository.Get(pid);

            return PartialView(prod);
        }
        public JsonResult ImgPreview(int pimgid)
        {
            _pRepository.SetPreview(pimgid);
            return Json("");
        }

        public static bool CheckPreview(int pimgid)
        {
            IProductRepository rep = new ProductRepository();
            var check = rep.CheckPreview(pimgid);
            return check;
        }
        public static string GetCatName(int id)
        {
            ICategoryRepository service = new CategoryRepository();
            var item = service.Get(id);
            return item.CategoryName;
        }
        [HttpPost]
        public JsonResult DelPhoto(int pimgid)
        {
            var img = _pRepository.GetImg(pimgid);
            _pRepository.PhotoDel(img);
            return Json(new { id = img.ID });
        }
        [HttpPost]
        public JsonResult SetPreview(int pimgid)
        {

            _pRepository.SetPreview(pimgid);
            var prodImg = _pRepository.GetImg(pimgid);
            var prod = _pRepository.Get(prodImg.ProductID);
            return Json(new { pimgid = pimgid, pid = _pRepository.GetImg(pimgid).ProductID });
        }
        public ActionResult Products(

            string catFilter,
            string sizeFilter,
            string priceFilter,
            int page = 1,

            string searchedTerm = null)
        {

            ProductListViewModel vm = new ProductListViewModel();
            List<Product> res1 = new List<Product>();
            List<Product> res2 = new List<Product>();
            List<Product> res3 = new List<Product>();
            List<Product> finalQuery = new List<Product>();
            string[] arr1 = null;
            string[] arr2 = null;
            string[] arr3 = null;
            var prods = _pRepository.Products.ToList();
            if (searchedTerm != null || !string.IsNullOrWhiteSpace(searchedTerm))
            {
                var items = prods
                        .Where(x =>
                            x.ProductType.ToLower().Contains(searchedTerm)
                        );
                vm = new ProductListViewModel
                {

                    Products = items
                        .OrderBy(x => x.ID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        Service = "Product",
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = items.Count()

                    }
                };


                return PartialView(vm);
            }
            if (catFilter != null)
            {
                arr1 = catFilter.Split(',');
            }
            if (Request.QueryString["size"] != null || sizeFilter != null)
            {
                arr2 = sizeFilter.Split(',');
            }
            if (Request.QueryString["price"] != null || priceFilter != null)
            {
                arr3 = priceFilter.Split(',');
            }
            if (catFilter == null && sizeFilter == null && priceFilter == null)
            {

                vm = new ProductListViewModel
                {

                    Products = _pRepository.Products
                        .OrderBy(x => x.ID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        Service = "Product",
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = _pRepository.Products.Count()

                    }
                };
                return PartialView(vm);
            }

            finalQuery = FilterCatalogItems(arr1, arr2, arr3);
            vm = new ProductListViewModel
            {
                Products = finalQuery
                    .OrderBy(x => x.ID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    Service = "Product",
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = finalQuery.Count()

                }
            };


            return PartialView(vm);

        }
        public List<Product> FilterCatalogItems(string[] arr1 = null, string[] arr2 = null, string[] arr3 = null)
        {

            var prods = _pRepository.Products.ToList();
            List<Product> res1 = new List<Product>();
            var num1 = 0;
            var num2 = 0;
            if (arr3 != null)
            {
                num1 = Int32.Parse(arr3[0]);
                num2 = Int32.Parse(arr3[1]);
            }
            if (arr3 != null && arr2 != null && arr1 != null)
            {
                res1 = prods
                    .Where(x => x.Price > num1 && x.Price < num2)
                    .Where(x => arr2.Intersect(x.Size.Split()).Count() > 0)
                    .Where(x => arr1.Contains(x.CategoryID.ToString()))
                    .ToList();
            }
            else if (arr3 != null && arr2 != null && arr1 == null)
            {
                res1 = prods
                    .Where(x => x.Price > num1 && x.Price < num2)
                    .Where(x => arr2.Intersect(x.Size.Split()).Count() > 0)
                    .ToList();
            }
            else if (arr3 != null && arr2 == null && arr1 != null)
            {
                res1 = prods
                    .Where(x => x.Price > num1 && x.Price < num2)
                    .Where(x => arr1.Contains(x.CategoryID.ToString()))
                    .ToList();
            }
            else if (arr3 != null && arr2 == null && arr1 == null)
            {
                res1 = prods
                    .Where(x => x.Price > num1 && x.Price < num2)
                    .ToList();
            }
            else if (arr3 == null && arr2 != null && arr1 == null)
            {
                res1 = prods
                    .Where(x => arr2.Contains(x.Size))
                    .Where(x => arr1.Contains(x.CategoryID.ToString()))
                    .ToList();
            }
            else
            {
                res1 = prods;
            }

            return res1;
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}
