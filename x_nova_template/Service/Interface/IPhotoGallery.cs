using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IPhotoGallery
    {
        Image GetImage(int id);
        Gallery GetGallery(int id);

        IQueryable<Gallery> Galleries { get; }
        IQueryable<Image> Images { get; }
        IQueryable<Gallery> GalAll();

        void Create(Gallery gallery = null, HttpPostedFileBase file = null,int galId=0);
        void Edit(Gallery gallery = null, Image image = null, HttpPostedFileBase file = null, int galId = 0);

        void Save();

        void Delete(Gallery gallery = null,Image image=null);
        

    }
}