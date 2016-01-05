using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;

namespace x_nova_template.Service.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Category> Categories { get { return db.Categories; } }
       

        public void Create(CategoryViewModel post)
        {
            Category cat = new Category();
            cat.CatType = post.CatType;
            cat.CatDescription = post.CatDescription;
            cat.CategoryName = post.CategoryName;
            cat.Sequance = post.Sequance;
            db.Categories.Add(cat);
            db.SaveChanges();
        }
        public IQueryable<Category> GetForSlider()
        {
            return from obj in db.Categories.Include("Products") select obj ;
        }
        public Category Get(int id)
        {
            var item = db.Categories.Find(id);
            return item;
        }
        public void Edit(CategoryViewModel post)
        {
            var cat = Get(post.ID);
            cat.CatType =post.CatType;
            cat.CatDescription = post.CatDescription;
            cat.CategoryName = post.CategoryName;
            cat.Sequance = post.Sequance;
            db.Entry(cat).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Category post)
        {
            db.Categories.Remove(post);
            db.SaveChanges();
        }
    }
}