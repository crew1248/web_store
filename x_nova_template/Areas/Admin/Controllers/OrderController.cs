using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;
using x_nova_template.ViewModel;

namespace x_nova_template.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepo;
        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        //
        // GET: /Admin/Order/

        public ActionResult Index(int page = 1, string sort = null)
        {
            int pageSize = 15;
            var orderList = (sort == null ? _orderRepo.Orders : _orderRepo.Orders.Where(x => x.OrderStatus == sort));
            OrderViewModel vm = new OrderViewModel
            {
                Orders = orderList
                .OrderBy(x => x.Sequance)
                .ThenByDescending(x => x.CreatedAt)
                .ToList()

                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = orderList.Count(),
                    Sort = sort
                }
            };
            return View(vm);
        }

        //
        // GET: /Admin/Order/Details/5

        public ActionResult Details(int id)
        {
            Order order = _orderRepo.Get(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(new OrderViewModel
            {
                Order = order,
                OrderStatus = order.OrderStatus

            });
        }

        //
        // POST: /Admin/Order/Delete/5



        public ActionResult Delete(int id, int Npage = 1)
        {
            Order order = _orderRepo.Get(id);
            _orderRepo.Delete(order);
            return RedirectToAction("Index", new { page = Npage });
        }

        [HttpPost]
        public JsonResult ChangeStatus(int orderId, string orderStatus)
        {
            var order = _orderRepo.Get(orderId);
            order.OrderStatus = orderStatus;
            switch (orderStatus)
            {
                case ("Не просмотрено"):
                    order.Sequance = 1; break;
                case ("Отменено"):
                    order.Sequance = 4; break;
                case ("На рассмотрении"):
                    order.Sequance = 3; break;
                case ("Ожидает подтверждения"):
                    order.Sequance = 2; break;
                case ("Отложено"):
                    order.Sequance = 5; break;

            }
            _orderRepo.Update();
            return Json(new { Name = orderStatus, Seq = order.Sequance });
        }

        public enum statusList
        {
            nepros,
            otlog,
            narasm,
            otmen,
            ogidaet
        }
    }
}