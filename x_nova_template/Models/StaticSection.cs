using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    public class StaticSection
    {
        [Key]
        public int ID { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public int Sequance {get;set;}

        [AllowHtml]
        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; }
        [StringLength(500)]
        public string Preview { get; set; }
        
        public int Type { get; set; }
        public int SectionType { get; set; }

       
    }
}