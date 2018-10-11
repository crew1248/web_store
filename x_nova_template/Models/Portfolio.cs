using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace x_nova_template.Models
{
    public class Portfolio
    {
        public int ID { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        [StringLength(200, ErrorMessage = "макс 100 слов")]
        [Required(ErrorMessage = "обязательно к заполнению")]
        public string Title { get; set; }
        [StringLength(250, ErrorMessage = "макс 150 слов")]

        public string Description { get; set; }

        public int? Price { get; set; }
        public int? Sortindex { get; set; }
        public string ProdLink { get; set; }

        public Portfolio(object str)
        {

        }
        public Portfolio() { }
    }
}