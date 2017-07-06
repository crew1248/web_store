using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.ViewModel
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public int ID { get; set; }
        
        public string CategoryName { get; set; }

        public string CatType { get; set; }
        [Range(0,500)]
        public int? Sequance { get; set; }
        public string CatDescription { get; set; }

        public int Sortindex { get; set; }
    }
}