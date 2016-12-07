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
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public float Price { get; set; }

        public string CategoryName { get; set; }
        [StringLength(500)]
        public string MatProd { get; set; }
        [StringLength(500)]
        public string Hardness { get; set; }
        [StringLength(500)]
        public string Channel { get; set; }
        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }


        [StringLength(500)]
        public string MatIronForm { get; set; } // Марка стали формообразующих частей
        [StringLength(500)]
        public string Block { get; set; } // Блок
        public string ProdTime { get; set; }  // Время производства
        public string Coupling { get; set; }  // Система охлаждения

        [StringLength(500)]
        public string Size { get; set; }
        [StringLength(500)]
        public string MatForm { get; set; }
        [StringLength(500)]
        public string Season { get; set; }
        [StringLength(500)]
        public string ProductType { get; set; }

       
    }
}