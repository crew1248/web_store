using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IOrderItemRepository
    {
        IQueryable<OrderItem> OrderItems { get; }

        void Create(Product prod, int quantity, int orderId,string cloth,string color);

        OrderItem Get(int id);
        void Delete(OrderItem orderItem);
        void Edit(OrderItem orderItem);
    }
}