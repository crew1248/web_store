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
        private FileManager filemanager = new FileManager();

        public IQueryable<Gallery> GalAll() {
            return from obj in db.Galleries.Include("Images") select obj;
        }

        public void Create(Gallery gallery = null,HttpPostedFileBase file = null,int galId=0)
        {
          
           
            if (gallery != null)
            {
                var gall = new Gallery();
                gall.GalleryMimeType = file.ContentType;
               
                gall.GalleryTitle = gallery.GalleryTitle;
               
                this.db.Galleries.Add(gall); 
                db.SaveChanges();
                gall.Sortindex = gall.ID;
                db.SaveChanges();
                int res = SavePhoto(file, gall.ID, true);
            }
            else {
                            
                Image image2 = new Image();                              
               
                image2.GalleryID = galId;                                            
                this.db.Images.Add(image2);
                db.SaveChanges();
                image2.Sortindex = image2.ID;
                db.SaveChanges();
                int res = SavePhoto(file, image2.ID, false);
            }
          
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
        public void UpdateSort(int id, int newSort)
        {
            Image image = GetImage(id);
            
            image.Sortindex = newSort;
            
            this.db.SaveChanges();
        }
        public void Save() {
            db.SaveChanges();
        }

        public int SavePhoto(HttpPostedFileBase file, int id,bool isGal=true)
        {
            
            if (file.ContentLength > 4000000) throw new HttpException();

            int imagesCount = 0;
            string dirPath = null;
            string dirPaths = null;
            string rndName = null;
            string filePath = null;
            string filePaths = null;
            byte[] istream = null;
            int resCount = 0;

           
         

            if (isGal)
            {
                var gal = GetGallery(id);
                dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + id);
                dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + id + "/200x150");
                imagesCount = filemanager.CheckDirectory(dirPath);
                rndName = filemanager.GetRandomName(imagesCount);
                gal.GalleryMimeType= rndName + Path.GetExtension(file.FileName);
            }
            else {
                var galImg = GetImage(id);
                dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + galImg.GalleryID+ "/Images");
                dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + galImg.GalleryID + "/Images/200x150");
                imagesCount = filemanager.CheckDirectory(dirPath);
                rndName = filemanager.GetRandomName(imagesCount);
                galImg.ImageMimeType = rndName + Path.GetExtension(file.FileName);
                resCount = id;
            }

           
            filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

            imagesCount = filemanager.CheckDirectory(dirPaths);
            filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));
            istream = new WebImage(file.InputStream).Resize(1921, 1081, true, true).Crop(1,1).GetBytes();

            filemanager.WriteImage(istream, filePath);
            istream = new WebImage(istream).Resize(201, 151, false, true).Crop(1, 1).GetBytes();
            filemanager.WriteImage(istream, filePaths);

           

            db.SaveChanges();

            return resCount;
        }


        public void DeleteAll(int id)
        {
            var Prodimg = db.Galleries.SingleOrDefault(x => x.ID == id);

            //var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" +id + "/Images/");
            //DirectoryInfo di = new DirectoryInfo(dirPaths);
            //di.Delete(true);

            foreach (var img in Prodimg.Images) {
                db.Images.Remove(img);
                db.SaveChanges();
            }
            
           
        }
        //public void PhotoDel(ProdImage pimg)
        //{
        //    var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
        //    var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/" + Prodimg.ImageMimeType);
        //    var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/200x150" + "/" + Prodimg.ImageMimeType);
        //    filemanager.RemoveFile(dirPath);
        //    filemanager.RemoveFile(dirPaths);

        //    db.ProdImages.Remove(pimg);
        //    db.SaveChanges();
        //}
        //public void Delete(Product product)
        //{
        //    var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + product.ID);
        //    filemanager.RemoveDir(dirPath);
        //    db.Products.Remove(product);
        //    db.SaveChanges();
        //}
        public void Delete(Gallery gallery = null, Image image = null)
        {
            if (image != null)
            {
                var img = db.Images.SingleOrDefault(x => x.ID == image.ID);
                var filePath = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + img.GalleryID + "/" + img.ImageMimeType);
                filemanager.RemoveFile(filePath);

                db.Images.Remove(image);
                db.SaveChanges();
            }
            else {
                var gal = db.Galleries.SingleOrDefault(x => x.ID == gallery.ID);
                var filePath = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" + gal.ID );
                filemanager.RemoveDir(filePath);
                db.Galleries.Remove(gallery);
                db.SaveChanges();
            }
        }
    }
}