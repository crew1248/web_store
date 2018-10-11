using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.Service.Interface
{
    public interface ISliderRepository
    {
        IQueryable<Portfolio> Sliders { get; }
        IEnumerable<Portfolio> getAll();

        void Create(Portfolio folio = null, HttpPostedFileBase file = null);
        void Edit(Portfolio folio = null, HttpPostedFileBase file = null);
        Portfolio GetPortfolio(int id);
        void Save();
        void Delete(Portfolio folio = null);
    }
}