using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface IMenuRepository
    {
        IQueryable<Menu> Menues { get; }

        void Create(Menu menu);

        Menu Get(int id);
        void Delete(Menu menu);
        void Edit(Menu menu);
        void UpdateSort(int id, int newSort);
    }
}