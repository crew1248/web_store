using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IOrderStatusRepository
    {
        IQueryable<OrderStatus> OrderStatuses { get; }

        void Create(OrderStatus orderStatus);

        OrderStatus Get(int id);
        void Delete(OrderStatus orderStatus);
        void Edit(OrderStatus orderStatus);
    }
}