using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IPhotoGallery
    {
        void Create(Gallery gallery = null, HttpPostedFileBase file = null, int galId = 0);
        void Delete(Gallery gallery = null, Image image = null);
        void DeleteAll(int id);
        void Edit(Gallery gallery = null, Image image = null, HttpPostedFileBase file = null, int galId = 0);
        IQueryable<Gallery> GalAll();
        Gallery GetGallery(int id);
        Image GetImage(int id);
        void Save();
        void UpdateImage(int id, string title);
        void UpdateSort(int id, int oldSort, int newSort);

        IQueryable<Gallery> Galleries { get; }

        IQueryable<Image> Images { get; }
    }
}