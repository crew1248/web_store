using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;

namespace x_nova_template.ViewModel
{
    [Serializable]
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        public Order Order { get; set; }

        public string OrderStatus { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
        public int Items { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<SelectListItem> StatusList
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem {Text="Не просмотрено"},
                    new SelectListItem {Text="На рассмотрении"},
                    new SelectListItem {Text="Ожидает подтверждения"},
                    new SelectListItem {Text="Отложено"},
                    new SelectListItem {Text="Отменено"}
                };

            }
        }
        public IEnumerable<SelectListItem> OrderFilters
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem {Text="Не просмотрено"},
                    new SelectListItem {Text="На рассмотрении"},
                    new SelectListItem {Text="Ожидает подтверждения"},
                    new SelectListItem {Text="Отложено"},
                    new SelectListItem {Text="Отменено"}
                };

            }
        }

    }
}