using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.ViewModel
{
    [Serializable]
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int TotalProducts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }

        public string GetImgLink(int id, string name)
        {

            return "/Content/Files/Product/" + id + "/" + name;
        }
    }
}