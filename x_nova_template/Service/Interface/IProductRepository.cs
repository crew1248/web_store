using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.ViewModel;

namespace x_nova_template.Service.Interface
{
    public interface IProductRepository
    {
         IQueryable<Product> Products { get; }
         IQueryable<ProdImage> ProdImages {get;}

         void Create(Product menu,ProductViewModel prod=null);
         void SetPreview(int pimgid);
         int SavePhoto(HttpPostedFileBase photo, int pid);
         ProdImage GetImg(int pimgid);
         bool GetPreviewImg(int pid);
         bool CheckPreview(int pimgid);
         void PhotoDel(ProdImage pimg);
         Product Get(int id);
         IEnumerable<Product> Get();
         void Delete(Product menu);
         void Edit(Product menu);
         void Save(); 
    }
}