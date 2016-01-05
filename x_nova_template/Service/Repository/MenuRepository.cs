using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class MenuRepository:IMenuRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Menu> Menues{ get { return db.Menues; } }


        public void Create(Menu menu) {
            menu.LastModifiedDate = DateTime.Now;
            db.Menues.Add(menu);            
            db.SaveChanges();
        }
        
        public Menu Get(int id) {
            var item = db.Menues.Find(id);
            return item;
        }
        public List<Menu> Get()
        {
            var item = db.Menues.ToList();
            return item;
        }
        public void Edit(Menu menu)
        {
            menu.LastModifiedDate = DateTime.Now;
            db.Entry(menu).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Menu menu)
        {
            db.Menues.Remove(menu);
            db.SaveChanges();
        }

       
    }
}