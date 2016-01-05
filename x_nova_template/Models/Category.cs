using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace x_nova_template.Models
{
    [Serializable]
    public class Category
    {
        public int ID { get; set; }
        [StringLength(300)]
        public string CategoryName { get; set; }

        public virtual List<Product> Products { get; set; }
        
        public int? Sequance { get; set; }
        public string CatDescription { get; set; }
        [StringLength(300)]
        public string CatType { get; set; }
        public int Sortindex { get; set; }
    }
}