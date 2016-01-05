using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Order> Orders { get { return db.Orders.Include("OrderItems"); } }

        public void Create(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public Order Get(int id)
        {
            var item = Orders.SingleOrDefault(x=>x.ID==id);
            return item;
        }
        public void Edit(Order order)
        {
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Order order)
        {
            db.Orders.Remove(order);
            db.SaveChanges();
        }
        public void Update() {
            db.SaveChanges();
        }
    }
}