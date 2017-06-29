using System;
using System.Collections.Generic;
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
        [StringLength(500)]
        public string MatForm { get; set; } // материал формообразующих
        [StringLength(500)]
        public string Size { get; set; } // размер
        [StringLength(500)]
        public string MatProd { get; set; } // материал изделия
        [StringLength(500)]
        public string Hardness { get; set; } // Жесткость
        [StringLength(500)]
        public string Channel { get; set; } // Канал
        [StringLength(500)]
        public string MatIronForm { get; set; } // Марка стали формообразующих частей
        [StringLength(500)]
        public string Block { get; set; } // Блок
        [StringLength(500)]
        public string ProdTime { get; set; }  // Время производства

        [StringLength(500)]
        public string Coupling { get; set; }  // Система охлаждения

        public Category Category { get; set; }

      
        [StringLength(500)]
        public string Composition { get; set; } // состав
        [StringLength(500)]
        public string Season { get; set; } // Сезон
        [StringLength(500)]
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