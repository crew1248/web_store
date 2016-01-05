using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{

    public class CartController : Controller
    {
        //
        // GET: /Admin/Cart/

        IProductRepository _repo;
        public CartController(IProductRepository repo)
        {

            _repo = repo;
        }

        [x_nova_template.Filters.RequireHttps(RequireSecure = false)]
        public ActionResult Index()
        {
            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });
        }
        public ActionResult CartSummary()
        {
            return PartialView(GetCart());
        }
        public ActionResult CartPartial()
        {

            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });

        }
        public ActionResult ViewCart(int prodId = 0)
        {

            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });

        }
        [HttpPost]
        public JsonResult Clear(Cart cart)
        {
            
            cart.Clear();
            return Json("");
        }
        public JsonResult UpdateItem(int pid,int type) {
            
            var prod = _repo.Get(pid);
           
            var newQ = type == 0 ? --GetCart().GetLine(pid).Quantity : ++GetCart().GetLine(pid).Quantity;
            if (GetCart().GetLine(pid).Quantity == 0)
                newQ = ++GetCart().GetLine(pid).Quantity;
            if (GetCart().GetLine(pid).Quantity == 20)
                newQ = --GetCart().GetLine(pid).Quantity;
            //GetCart().ChangeQuantity(prod, newQ);

            return Json(new { s = GetCart().TotalValue().ToString("000"), t = newQ, quant = GetCart().GetLine(pid).Quantity,price=prod.Price });
        }
        public ActionResult NewItem(int prodId)
        {
            var prod = _repo.Get(prodId);
            return PartialView(prod);
        }
        [HttpPost]
        public JsonResult AddToCart(int prodId)
        {
            var prod = _repo.Products.FirstOrDefault(x => x.ID == prodId);
            if (prod != null)
            {
                GetCart().AddItem(prod, 1);
            }
            //Guid.NewGuid().ToString();
            return Json(new
            {
                total = GetCart().TotalValue(),
                count = GetCart().Lines.Count(),
                price = prod.Price,
                title = prod.ProductName,
                cid = Guid.NewGuid().ToString()
            });
        }
        [HttpPost]
        public JsonResult ChangeQuantity(int prodId, int quant)
        {
            var prod = _repo.Get(prodId);
            GetCart().ChangeQuantity(prod, quant);
            return Json(new { itemTotal = (prod.Price * quant).ToString("0 000") });
        }
        public JsonResult GetCartSummary()
        {
            var val = string.Format("{0}", GetCart().TotalValue().ToString("0 000"));
            return Json(new
            {
                count = GetCart().Lines.Count(),
                total = (GetCart().Lines.Count() == 0 ? "0" : val)
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartialCartRow(int prodId)
        {
            var prod = _repo.Get(prodId);
            return PartialView(prod);

        }
        [HttpPost]
        public JsonResult RemoveFromCart(int prodId)
        {
            
            var prod = _repo.Products.FirstOrDefault(x => x.ID == prodId);
            if (prod != null)
            {
                GetCart().RemoveLine(prod);


                return Json(new
                {
                    id = prodId,
                    total = GetCart().TotalValue(),
                    count = GetCart().Lines.Count(),
                });
            }
            else throw new HttpException();
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

        public bool AddedProdId(int id)
        {
            IProductRepository r = new ProductRepository();

            var added = GetCart().Lines.Select(x => x.Product.ID).Contains(id);
            return added;
        }

    }
}
