using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.ViewModel;

namespace x_nova_template.Service.Repository
{
    public class ProductRepository:IProductRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Product> Products { get { return db.Products.Include("ProdImages"); } }
        public IQueryable<ProdImage> ProdImages { get { return db.ProdImages; } }

        public void Create(Product product,ProductViewModel prod=null)
        {
           
                if (product != null)
                {
                    db.Products.Add(product);
                }
                else
                {
                    var newP = new Product();
                    newP.ProductName = prod.ProductName;
                    newP.Description = prod.Description;
                    newP.Season = prod.Season;
                    newP.Size = prod.Size;
                    newP.ProductType = prod.ProductType;
                    newP.Price = prod.Price;
                    newP.CategoryID = prod.CategoryID;
                    db.Products.Add(newP);
                }
                db.SaveChanges();
            
        }
        public IEnumerable<Product> Get() {
            return from obj in Products select obj;
        }
        public int SavePhoto(HttpPostedFileBase photo,int pid) {
            var prodImg = new ProdImage();
            prodImg.ProductID = pid;
            prodImg.ImageDataType = new BinaryReader(photo.InputStream).ReadBytes(photo.ContentLength);
            prodImg.ImageMimeType = photo.ContentType;
            db.ProdImages.Add(prodImg);
            db.SaveChanges();
            return prodImg.ID;
        }
        public void SetPreview(int pimgid) {

            var prodImg = db.ProdImages.Find(pimgid);
            var prod = Get(prodImg.ProductID);
            prod.ProdImages.ForEach(x => x.IsPreview = 0);
            prodImg.IsPreview = 1;
            db.SaveChanges();
        }
        public ProdImage GetImg(int pimgid) {
            var prodImg = db.ProdImages.Find(pimgid);
            return prodImg;
        }
        public bool GetPreviewImg(int pid) {
            var prod = Get(pid);
            return prod.ProdImages.Any(x=>x.IsPreview==1);
        }
        public bool CheckPreview(int pimgid) {
            return GetImg(pimgid).IsPreview == 1 ? true : false;
        }
        public void PhotoDel(ProdImage pimg) {
            
            db.ProdImages.Remove(pimg);
            db.SaveChanges();

        }
        public void Save() {
            db.SaveChanges();
        }

        public Product Get(int id)
        {
            var item = db.Products.Find(id);
            return item;
        }
        public void Edit(Product product)
        {
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}