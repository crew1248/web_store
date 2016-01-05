using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class PostRepository:IPostRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Post> Posts { get { return db.Posts; } }

        public void Create(Post post,HttpPostedFileBase file)
        {
            post.PreviewPhoto = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
            db.Posts.Add(post);
            db.SaveChanges();
        }

        public Post Get(int id)
        {
            var item = db.Posts.Find(id);
            return item;
        }
        public void Edit(Post post, HttpPostedFileBase file=null)
        {
            if (file != null)
            {
                post.PreviewPhoto = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                db.Entry(post).State = System.Data.Entity.EntityState.Modified;
            }
            else {
                var item = Get(post.ID);
                item.Title = post.Title;
                item.Body = post.Body;
                item.Preview = post.Preview;
            }
            db.SaveChanges();
        }
        public void Delete(Post post)
        {
            db.Posts.Remove(post);
            db.SaveChanges();
        }
    }
}