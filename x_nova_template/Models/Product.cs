using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    [Serializable]    
    public class Product
    {
        public int ID { get; set; }
        [StringLength(300)]
        public string ProductName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(300)]
        public string Size { get; set; }
        [StringLength(300)]
        public string Composition { get; set; }
        [StringLength(300)]
        public string Season { get; set; }
        [StringLength(300)]
        public string ProductType { get; set; }
        public int Sortindex { get; set; }
        public virtual List<ProdImage> ProdImages { get; set; }

        public int CategoryID { get; set; }

        public float Price { get; set; }
     
    }
}