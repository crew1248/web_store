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
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FileManager filemanager = new FileManager();


        public IQueryable<Product> Products { get { return db.Products.Include("ProdImages"); } }
        public IQueryable<ProdImage> ProdImages { get { return db.ProdImages; } }

        public void Create(Product product, ProductViewModel prod = null)
        {

            if (product != null)
            {
                db.Products.Add(product);
            }
            else
            {
                var newP = new Product();
                newP.Sortindex = newP.ID + 1;
                newP.ProductName = prod.ProductName;
                newP.Description = prod.Description;
                newP.MatForm = prod.MatForm;
                newP.MatIronForm = prod.MatIronForm;
                newP.ProdTime = prod.ProdTime;
                newP.Block = prod.Block;
                newP.Season = prod.Season;
                newP.Coupling = prod.Coupling;
                newP.MatProd = prod.MatProd;
                newP.Channel = prod.Channel;
                newP.Hardness = prod.Hardness;
                newP.Size = prod.Size;
                newP.ProductType = prod.ProductType;
                newP.Price = prod.Price;
                newP.CategoryID = prod.CategoryID;
                db.Products.Add(newP);
            }
            db.SaveChanges();

        }
        public IEnumerable<Product> Get()
        {
            return from obj in Products select obj;
        }
        public int SavePhoto(HttpPostedFileBase file, int pid)
        {

            if (file.ContentLength > 4000000) throw new HttpException();

            int imagesCount = 0;
            var prodImg = new ProdImage();

            prodImg.ProductID = pid;
            db.ProdImages.Add(prodImg);
            db.SaveChanges();
            prodImg.Sortindex = prodImg.ID;

            var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pid);
            var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pid + "/200x150");

            imagesCount = filemanager.CheckDirectory(dirPath);
            var rndName = filemanager.GetRandomName(imagesCount);
            var filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

            imagesCount = filemanager.CheckDirectory(dirPaths);
            var filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));
            var istream = new WebImage(file.InputStream).Resize(1921, 1081, true, true).Crop(1, 1).GetBytes();

            filemanager.WriteImage(istream, filePath);
            istream = new WebImage(istream).Resize(301, 211, false, true).Crop(1, 1).GetBytes();
            filemanager.WriteImage(istream, filePaths);

            prodImg.ImageMimeType = rndName + Path.GetExtension(file.FileName);

            db.SaveChanges();

            return prodImg.ID;
        }
        public void SetPreview(int pimgid)
        {

            var prodImg = db.ProdImages.Find(pimgid);
            var prod = Get(prodImg.ProductID);
            prod.ProdImages.ForEach(x => x.IsPreview = 0);
            prodImg.IsPreview = 1;
            db.SaveChanges();
        }
        public ProdImage GetImg(int pimgid)
        {
            var prodImg = db.ProdImages.Find(pimgid);
            return prodImg;
        }
        public bool GetPreviewImg(int pid)
        {
            var prod = Get(pid);
            return prod.ProdImages.Any(x => x.IsPreview == 1);
        }

        public void UpdateSort(int id, int oldSort, int newSort)
        {
            var curr = this.Get(id);
            ProdImage image = this.ProdImages.Where(x => x.ProductID == id).SingleOrDefault<ProdImage>(x => x.Sortindex == oldSort);
            ProdImage image2 = this.ProdImages.Where(x => x.ProductID == id).SingleOrDefault<ProdImage>(x => x.Sortindex == newSort);
            image.Sortindex = newSort;
            image2.Sortindex = oldSort;
            db.SaveChanges();
        }
        public bool CheckPreview(int pimgid)
        {
            return GetImg(pimgid).IsPreview == 1 ? true : false;
        }
        public void PhotoDel(ProdImage pimg)
        {
            var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
            var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/" + Prodimg.ImageMimeType);
            var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/200x150" + "/" + Prodimg.ImageMimeType);
            filemanager.RemoveFile(dirPath);
            filemanager.RemoveFile(dirPaths);

            db.ProdImages.Remove(pimg);
            db.SaveChanges();
        }
        public void Save()
        {
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
            var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + product.ID);
            filemanager.RemoveDir(dirPath);
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}