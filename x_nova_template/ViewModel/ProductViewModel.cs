using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.ViewModel
{
    [Serializable]
    public class ProductViewModel
    {

        public ProductViewModel(){
            Fill = "высоко эластичный ППУ";
    }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        public string ProductName { get; set; }
        public string imgLink { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public float Price { get; set; }
        public string CategoryName { get; set; }
        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }        
        [StringLength(300)]
        public string ProductType { get; set; }



        public string Size { get; set; } // размеры
        public string Packaging { get; set; } // упак
        public float PackagingSize { get; set; } //размер упак
        public float Weight { get; set; } // вес
        public string Manufacturer { get; set; } //производитель
        public string Cloth { get; set; }//ткань
        public string Color { get; set; } //окрас
        public string Lacquering { get; set; }//лакировка
        public string Decor { get; set; } //декор
        public int Discount { get; set; } //скидка
        public string Material { get; set; }//материал
        
       
        public string Fill { get; set; }//наполнение


    }
}