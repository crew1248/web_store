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
            int newW = 0;
            int newH = 0;
            if (gallery != null)
            {
                var gall = new Gallery();
                gall.GalleryMimeType = file.ContentType;
                newdata = new WebImage(file.InputStream).Resize(800, 600).GetBytes();
               
                gall.GalleryTitle = gallery.GalleryTitle;
                gall.GalleryData = newdata;
                //new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                db.Galleries.Add(gall);
            }
            else {
                var img = new Image();
                var newString = System.Text.RegularExpressions.Regex.Replace(file.FileName, @"\.\w+", string.Empty);
                img.ImageTitle = newString;
                img.GalleryID = galId;
                img.ImageMimeType = file.ContentType;
                newdata = new WebImage(file.InputStream).Resize(800, 600).GetBytes();
                img.ImageData = newdata;
                db.Images.Add(img);
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
                    newdata = new WebImage(file.InputStream).Resize(800, 600).GetBytes();
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
                    newdata = new WebImage(file.InputStream).Resize(800, 600).GetBytes();
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

        public void Save() {
            db.SaveChanges();
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