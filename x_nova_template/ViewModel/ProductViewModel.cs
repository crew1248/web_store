using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.ViewModel
{
    [Serializable]
    public class ProductViewModel
    {
        [HiddenInput(DisplayValue=false)]       
        public int ID { get; set; }
        
        public string ProductName { get; set; }

        public string Description { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public float Price { get; set; }
        
        public string CategoryName { get; set; }

        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }

        [StringLength(300)]
        public string Size { get; set; }
        [StringLength(300)]
        public string Composition { get; set; }
        [StringLength(300)]
        public string Season { get; set; }
        [StringLength(300)]
        public string ProductType { get; set; }

       
    }
}