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
    public class SliderRepository:ISliderRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<Portfolio> Sliders { get { return db.Portfolios; } }
        


        public IEnumerable<Portfolio> getAll()
        {
            return from obj in db.Portfolios select obj;
        }

        public void Create(Portfolio folio = null, HttpPostedFileBase file = null)
        {
            byte[] newdata=null;
            if (folio != null)
            {
                var gall = new Portfolio();
                gall.Title = folio.Title;
                gall.Description = folio.Description;
                gall.Price = folio.Price;
                gall.ImageMimeType = file.ContentType;
                newdata = new WebImage(file.InputStream).Resize(1280, 1024).GetBytes();
                gall.ImageData = newdata;
                //new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                db.Portfolios.Add(gall);
            }
            db.SaveChanges();
        }

        public void Edit(Portfolio folio = null, HttpPostedFileBase file = null)
        {
                byte[] newdata = null;
                var fol = db.Portfolios.Find(folio.ID);
                fol.Title = folio.Title;
                fol.Description = folio.Description;
                fol.Price = folio.Price;
                if (file != null)
                {
                    fol.ImageMimeType = file.ContentType;
                    newdata = new WebImage(file.InputStream).Resize(1280, 1024).GetBytes();
                    fol.ImageData = newdata;
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