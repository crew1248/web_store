using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UnidecodeSharpFork;

namespace x_nova_template.Models
{
    [Serializable]
    public class Product
    {
        public int ID { get; set; }
        [StringLength(500)]
        public string ProductName { get; set; }

        [AllowHtml]
        public string Description { get; set; } //описание
       
        public Category Category { get; set; }
         [StringLength(500)]
        public string Size { get; set; } // размеры
         [StringLength(500)]
        public string Packaging { get; set; } // упак
         
        public float PackagingSize {get;set;} //размер упак
    
        public float Weight { get; set; } // вес
         [StringLength(500)]
        public string Manufacturer { get; set; } //производитель
         [StringLength(500)]
        public string Cloth { get; set;  }//ткань
         [StringLength(500)]
        public string Color { get; set; } //окрас
         [StringLength(500)]
        public string Lacquering { get; set; }//лакировка
         [StringLength(500)]
        public string Decor { get; set; } //декор
        
        public int Discount { get; set; } //скидка
         [StringLength(500)]
        public string Material { get; set;  }//материал

         [StringLength(500)]         
         public string Fill { get; set; }//наполнение
         
         public int VisitesCount { get; set; }//просмотров товара



        public string ProductType { get; set; }
        public int Sortindex { get; set; }
        public virtual List<ProdImage> ProdImages { get; set; }

        public int CategoryID { get; set; }

        public float Price { get; set; }

        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", ID, ProductName);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 100 ? str.Length : 100).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            var unicoder = text.Unidecode();

            return unicoder;
        }



    }
}