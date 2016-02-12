using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class PhotoGalleryRepository:IPhotoGallery
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Image> Images { get { return db.Images; } }
        public IQueryable<Gallery> Galleries { get { return db.Galleries; } }


        public IQueryable<Gallery> GalAll() {
            return from obj in db.Galleries.Include("Images") select obj;
        }

        public void Create(Gallery gallery = null,HttpPostedFileBase file = null,int galId=0)
        {
            byte[] newdata = null;
           
            if (gallery != null)
            {
                var gall = new Gallery();
                gall.GalleryMimeType = file.ContentType;
                newdata = new WebImage(file.InputStream).GetBytes(null);
                gall.GalleryTitle = gallery.GalleryTitle;
                gall.GalleryData = newdata;
                this.db.Galleries.Add(gall);
            }
            else {
               
             
                Image image2 = new Image();
                string str = Regex.Replace(file.FileName, @"\.\w+", string.Empty);
                image2.ImageTitle = str;
                image2.Sortindex = image2.ID+1;
                image2.GalleryID = galId;
                image2.ImageMimeType = file.ContentType;
                newdata = new WebImage(file.InputStream).GetBytes(null);
                image2.ImageData = newdata;
                this.db.Images.Add(image2);
            }
           db.SaveChanges();
        }

        public void Edit(Gallery gallery=null, Image image = null, HttpPostedFileBase file=null,int galId=0) {
            byte[] newdata = null;
            if (gallery != null)
            {
                if (file != null)
                {
                    gallery.GalleryMimeType = file.ContentType;
                    newdata = new WebImage(file.InputStream).GetBytes();
                    gallery.GalleryData = newdata;
                    db.Entry(gallery).State = System.Data.Entity.EntityState.Modified;
                }
                else {
                    var gal = db.Galleries.Find(gallery.ID);
                    gal.GalleryTitle = gallery.GalleryTitle;                    
                }
                
            }
            else
            {
                if (file != null)
                {
                    image.ImageMimeType = file.ContentType;
                    newdata = new WebImage(file.InputStream).GetBytes();
                    image.ImageData =newdata;
                }
                db.Entry(image).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }
       
        public Image GetImage(int id) {
            var image = db.Images.Find(id);
            return image;
        }

        public Gallery GetGallery(int id)
        {
            var gallery = db.Galleries.Find(id);
            return gallery;
        }
        public void UpdateImage(int id, string title)
        {
            this.GetImage(id).ImageTitle = title;
            this.db.SaveChanges();
        }
        public void UpdateSort(int id, int oldSort, int newSort)
        {
            Image image = this.GetGallery(id).Images.SingleOrDefault<Image>(x => x.Sortindex == oldSort);
            Image image2 = this.GetGallery(id).Images.SingleOrDefault<Image>(x => x.Sortindex == newSort);
            image.Sortindex = newSort;
            image2.Sortindex = oldSort;
            this.db.SaveChanges();
        }
        public void Save() {
            db.SaveChanges();
        }

        public void DeleteAll(int id)
        {
            this.db.Database.ExecuteSqlCommand("Delete from Images where GalleryID =" + id, new object[0]);
            this.db.SaveChanges();
        }
        public void Delete(Gallery gallery = null, Image image = null)
        {
            if (image != null)
            {
                db.Images.Remove(image);
                db.SaveChanges();
            }
            else {
                db.Galleries.Remove(gallery);
                db.SaveChanges();
            }
        }
    }
}