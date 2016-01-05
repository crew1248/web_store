using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.ViewModel;

namespace x_nova_template.Service.Interface
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get;  }
        IQueryable<Category> GetForSlider();
        void Create(CategoryViewModel post);

        Category Get(int id);
        void Delete(Category post);
        void Edit(CategoryViewModel post);
    }
}