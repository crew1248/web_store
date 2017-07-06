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
            //Menu image = (from i in this.db.Images
            //               where i.Sortindex == (from x in this.db.Images select x.Sortindex).Max<int>()
            //               select i).SingleOrDefault<Image>();
            //int num = (image == null) ? 1 : (image.Sortindex + 1);


            db.Menues.Add(menu);
            db.SaveChanges();
            var res = db.Menues.Single(x => x.Url == menu.Url);
            res.SortOrder = res.Id + 1;
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
            menu.SortOrder = (menu.SortOrder == 0 || menu.SortOrder == 1 ? menu.Id + 1 : menu.SortOrder);
            db.Entry(menu).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Menu menu)
        {
            db.Menues.Remove(menu);
            db.SaveChanges();
        }

        public void UpdateSort(int id, int newSort)
        {
            Menu image = Get(id);

            image.SortOrder= newSort;

            this.db.SaveChanges();
        }
    }
}