using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class OrderItemRepository:IOrderItemRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<OrderItem> OrderItems { get { return db.OrderItems; } }

        public void Create(Product product,int quantity, int orderId)
        {
            OrderItem oitem = new OrderItem();

            oitem.OrderID = orderId;
            oitem.ProductName = product.ProductName;
            oitem.Quantity = quantity;
            oitem.Price = product.Price;
            oitem.Category = db.Categories.Find(product.CategoryID).CategoryName;
            oitem.Description = product.Description;
            db.OrderItems.Add(oitem);
            db.SaveChanges();
        }

        public OrderItem Get(int id)
        {
            var item = db.OrderItems.Find(id);
            return item;
        }
        public void Edit(OrderItem orderitem)
        {
            db.Entry(orderitem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(OrderItem orderitem)
        {
            db.OrderItems.Remove(orderitem);
            db.SaveChanges();
        }
    }
}