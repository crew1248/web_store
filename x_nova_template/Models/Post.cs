using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    public class Post
    {
        public int ID { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage="Заполните поле")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        [AllowHtml]
        public string Body { get; set; }

        public byte[] PreviewPhoto { get; set; }
        public DateTime CreatedAt { get; set; }

        [StringLength(500)]
        public string Preview { get; set; }
        public int Sortindex { get; set; }
    }
}