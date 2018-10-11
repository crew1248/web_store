using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using x_nova_template.Models;
using x_nova_template.Service.Interface;

namespace x_nova_template.Service.Repository
{
    public class SliderRepository : ISliderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FileManager filemanager = new FileManager();
        public IQueryable<Portfolio> Sliders { get { return db.Portfolios; } }



        public IEnumerable<Portfolio> getAll()
        {
            return from obj in db.Portfolios select obj;
        }

        public void Create(Portfolio folio = null, HttpPostedFileBase file = null)
        {

            if (folio != null)
            {
                var gall = new Portfolio();
                gall.Title = folio.Title;
                gall.Description = folio.Description;
                gall.Price = folio.Price;
                // gall.ImageMimeType = file.ContentType;


                if (file.ContentLength > 4000000) throw new HttpException();

                //new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                db.Portfolios.Add(gall);
                db.SaveChanges();

                int imagesCount = 0;


                gall.Sortindex = gall.ID;

                var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Slider/" + gall.ID);
                var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Slider/" + gall.ID + "/200x150");

                imagesCount = filemanager.CheckDirectory(dirPath);
                var rndName = filemanager.GetRandomName(imagesCount);
                var filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

                imagesCount = filemanager.CheckDirectory(dirPaths);
                var filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));
                var istream = new WebImage(file.InputStream).Resize(1921, 1081, true, true).Crop(1,1).GetBytes();

                filemanager.WriteImage(istream, filePath);
                istream = new WebImage(istream).Resize(201, 151, false, true).Crop(1,1).GetBytes();
                filemanager.WriteImage(istream, filePaths);

                gall.ImageMimeType = rndName + Path.GetExtension(file.FileName);

                db.SaveChanges();

            }




        }

        public void Edit(Portfolio folio = null, HttpPostedFileBase file = null)
        {

            var fol = db.Portfolios.Find(folio.ID);
            fol.Title = folio.Title;
            fol.Description = folio.Description;
            fol.Price = folio.Price;
            if (file != null)
            {
                if (file.ContentLength > 4000000) throw new HttpException();

                //new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

                int imagesCount = 0;

                var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Slider/" + fol.ID);
                var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Slider/" + fol.ID + "/200x150");



                imagesCount = filemanager.CheckDirectory(dirPath);
                filemanager.ClearDir(dirPath);
                var rndName = filemanager.GetRandomName(imagesCount);
                var filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

                imagesCount = filemanager.CheckDirectory(dirPaths);
                filemanager.ClearDir(dirPaths);
                var filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));
                var istream = new WebImage(file.InputStream).Resize(1920, 1080, true, true).GetBytes();

                filemanager.WriteImage(istream, filePath);
                istream = new WebImage(istream).Resize(200, 150, false, true).GetBytes();
                filemanager.WriteImage(istream, filePaths);

                fol.ImageMimeType = rndName + Path.GetExtension(file.FileName);

                db.SaveChanges();
            }

            db.SaveChanges();
        }



        public Portfolio GetPortfolio(int id)
        {
            var folio = db.Portfolios.Find(id);
            return folio;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Delete(Portfolio folio = null)
        {
            if (folio != null)
            {
                db.Portfolios.Remove(folio);
                db.SaveChanges();
            }
        }
    }
}