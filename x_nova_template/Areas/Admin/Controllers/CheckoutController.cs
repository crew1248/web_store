using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace x_nova_template.Areas.Admin.Controllers
{

    public class CheckoutController : Controller
    {
        //
        // GET: /Admin/Checkout/
        private IOrderProcessor _orderProcessor;
        private IProductRepository _product;
        private IOrderRepository _order;
        private IOrderStatusRepository _orderStatus;
        private IOrderItemRepository _orderItem;

        public CheckoutController
            (
                IOrderProcessor orderProcessor,
                IProductRepository product,
                IOrderRepository order,
                IOrderStatusRepository orderStatus,
                IOrderItemRepository orderItem
            )
        {
            _orderProcessor = orderProcessor;
            _product = product;
            _order = order;
            _orderStatus = orderStatus;
            _orderItem = orderItem;
        }

        //[x_nova_template.Filters.RequireHttps(RequireSecure = true)]
        public async Task<ActionResult> Index(int step)
        {
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user =await userManager.FindByNameAsync(User.Identity.Name);
            var cart = GetCart();
            var step2CheckInputs = cart.ClientDetails.HasEmptyProperties();
            
            ViewBag.Step1 = GetCart().Step1;
            ViewBag.Step2 = GetCart().Step2;
            ViewBag.Step3 = GetCart().Step3;
            ViewBag.Step4 = GetCart().Step4;
            
            if (cart.Lines.Count() == 0)
            {
                return Redirect("/");
            }
            // Process for Authenticated
            //
            if (User.Identity.IsAuthenticated) {
               

                if (step == 1)
                {
                    ViewBag.Step = 1;
                    
                    cart.Step2 = true;
                    return View();
                }
                else if (step == 2 && cart.Step2)
                {
                   
                    ViewBag.Step = 2;
                    //if (!step2CheckInputs) ModelState.AddModelError("", "Заполните все данные в вашей профиле");
                    cart.UpdateClientDetails(user);
                    return View(new CheckoutViewModel
                    {
                        Address = user.Address,
                        FirstName = user.Firstname,
                        LastName = user.Sirname,
                        Email = user.Email,
                        Phone = user.Phone,
                        Delivery = user.Delivery,
                        Payment = user.Payment
                    });
                    
                  

                }
                else if (step == 3 && cart.Step3)
                {
                    ViewBag.Step = 3;
                    cart.UpdateDelivery(user);
                    return View(new CheckoutViewModel { Delivery =user.Delivery });
                }
                else if (step == 4 && cart.Step4)
                {
                    ViewBag.Step = 4;
                    cart.UpdatePayment(user);
                    return View(new CheckoutViewModel { Payment =user.Payment});
                }
                else { return RedirectToAction("Index", new { step = 1 }); }
            }
            // Process for Anonymous
            //
            if (step == 1)
            {
                ViewBag.Step = 1;
                cart.Step2 = true;
                return View();
            }
            else if (step == 2 && cart.Step2)
            {
                ViewBag.Step = 2;


                //if (cart.ClientDetails.FirstName != null)
                //{
                    return View(new CheckoutViewModel
                    {
                        Address = cart.ClientDetails.Address,
                        FirstName = cart.ClientDetails.FirstName,
                        LastName = cart.ClientDetails.LastName,
                        Email = cart.ClientDetails.Email,
                        Phone = cart.ClientDetails.Phone,
                        Delivery = cart.ClientDetails.Delivery,
                        Payment = cart.ClientDetails.Payment
                    });
                //}
                //return View(new CheckoutViewModel());
            }
            else if (step == 3 && cart.Step3)
            {
                ViewBag.Step = 3;

                return View(new CheckoutViewModel { Delivery = cart.ClientDetails.Delivery });
            }
            else if (step == 4 && cart.Step4)
            {
                ViewBag.Step = 4;
                return View(new CheckoutViewModel { Payment = cart.ClientDetails.Payment });
            }
            else { return RedirectToAction("Index", new { step = 1 }); }
        }
        [HttpPost]
        public JsonResult Processing() {
            System.Threading.Thread.Sleep(1000);
            var t = (User.Identity.IsAuthenticated?1:0);
            return Json(new { type = t });
        }
        [HttpPost]
        public ActionResult ProceedCheckout(CheckoutViewModel vm)
        {

            if (ModelState.IsValid)
            {
                var cart = GetCart();
                cart.Step3 = true;
                cart.UpdateClientDetails(vm);
                return RedirectToAction("Index", new { step = 3 });
            }
            return RedirectToAction("Index", new { step = 2 });
        }
        [HttpPost]
        public ActionResult ProceedDelivery(Checkout_Delivery vm)
        {

            if (ModelState.IsValid)
            {

                var cart = GetCart();
                cart.Step4 = true;
                cart.UpdateDelivery(vm);
                return RedirectToAction("Index", new { step = 4 });
            }
            return RedirectToAction("Index", new { step = 3 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProceedPayment(Checkout_Payment vm)
        {
            if (ModelState.IsValid)
            {
                var cart = GetCart();
                var clientInfo = cart.ClientDetails;
                var step2CheckInputs = cart.ClientDetails.HasEmptyProperties();
                if (!step2CheckInputs) {
                    RedirectToAction("Checkout", new { step = 2 });
                }
                cart.UpdatePayment(vm);
                var order = new Order();
                order.Address = cart.ClientDetails.Address;
                order.Name = string.Format("{0} {1}", clientInfo.FirstName, clientInfo.LastName);
                order.Phone = clientInfo.Phone;
                order.OrderStatus = "Не просмотрено";
                order.Payment = clientInfo.Payment;
                order.CreatedAt = DateTime.Now;
                order.Delivery = clientInfo.Delivery;
                order.OrderSum = cart.TotalValue();
                order.Sequance = 1;
                _order.Create(order);
                foreach (var item in cart.Lines)
                {
                    _orderItem.Create(item.Product, item.Quantity, order.ID);
                }
                /*
                YaMoney ya = new YaMoney();
                string url = ya.GetTokenRequestURL();
                Response.Redirect(url);
                */
                cart.Clear();

                return RedirectToAction("Finished");
            }
            return RedirectToAction("Index", new { step = 4 });
        }
        //[x_nova_template.Filters.RequireHttps(RequireSecure = true)]
        public ActionResult Finished(bool transactionOk = false)
        {
            /* YaMoney ya = new YaMoney();
             if (transactionOk&&(string)Session["token"]!=null) {

                
                 string tokenId = (string)Session["token"];
                 ya.AccessToken = tokenId;

                 if (tokenId != null)
                 {
                  
                    ViewBag.AccInfo =  ya.ProcessPayment();
               
                 }


                // ViewBag.YaMessage = "Транзакция окончена успешно !";
             }*/
            return View();
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


        #region Single Order

        public IEnumerable<SelectListItem> GetYears()
        {
            List<SelectListItem> years = new List<SelectListItem>();
            for (var i = 1930; i <= 2000; i++)
            {
                years.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            IEnumerable<SelectListItem> formattedYears = years.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
            return formattedYears;
        }
        public IEnumerable<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>{
                
                new SelectListItem {Text="Январь",Value="1"},
                new SelectListItem {Text="Февраль",Value="2"},
                new SelectListItem {Text="Март",Value="3"},
                new SelectListItem {Text="Апрель",Value="4"},
                new SelectListItem {Text="Май",Value="5"},
                new SelectListItem {Text="Июнь",Value="6"},
                new SelectListItem {Text="Июль",Value="7"},
                new SelectListItem {Text="Август",Value="8"},
                new SelectListItem {Text="Сентябрь",Value="9"},
                new SelectListItem {Text="Октябрь",Value="10"},
                new SelectListItem {Text="Ноябрь",Value="11"},
                new SelectListItem {Text="Декабрь",Value="12"}
            };
            return months;


        }

        public IEnumerable<SelectListItem> GetDays()
        {
            List<SelectListItem> days = new List<SelectListItem>();

            for (var i = 1; i <= 31; i++)
            {
                days.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return days;
        }

        public ActionResult OrderPartial()
        {
            ViewBag.Year = GetYears();
            ViewBag.Month = GetMonths();
            ViewBag.Day = GetDays();
            return PartialView(new CheckoutViewModel());
        }

        [HttpPost]
        public ActionResult OrderPartial(CheckoutViewModel vm)
        {

            System.Threading.Thread.Sleep(1000);

            if (ModelState.IsValid)
            {
                var order = new Order();
                order.EmailAddress = vm.Email;
                order.Name = vm.Name;
                order.Phone = vm.Phone;
                order.OrderStatus = "Не просмотрено";
                order.CreatedAt = DateTime.Now;

                _order.Create(order);
                return PartialView("OrderOk");
            }
            throw new HttpException();
        }

        #endregion


    }
}
