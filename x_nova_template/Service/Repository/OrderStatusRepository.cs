using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class OrderStatusRepository:IOrderStatusRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<OrderStatus> OrderStatuses { get { return db.OrderStatuses; } }

        public void Create(OrderStatus orderStatus)
        {
            db.OrderStatuses.Add(orderStatus);
            db.SaveChanges();
        }

        public OrderStatus Get(int id)
        {
            var item = db.OrderStatuses.Find(id);
            return item;
        }
        public void Edit(OrderStatus orderStatus)
        {
            db.Entry(orderStatus).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(OrderStatus orderStatus)
        {
            db.OrderStatuses.Remove(orderStatus);
            db.SaveChanges();
        }
    }
}