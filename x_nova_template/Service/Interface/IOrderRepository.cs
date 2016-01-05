using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        void Create(Order order);

        Order Get(int id);
        void Delete(Order order);
        void Edit(Order order);
        void Update();
    }
}