using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    public class Menu
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните Id-родитель")]
        public int ParentId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните название")]
        public string Text { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните Url")]
        [RegularExpression(@"([a-zA-Z0-9]+(-|_)[-a-zA-Z0-9]+|\w+)", ErrorMessage = "допустимый формат ввода = o_kompanii или o-kompanii)")]
        public string Url { get; set; }

        [AllowHtml]
        public string Body { get; set; }
        [AllowHtml]
        public string BodyEng { get; set; }
        [AllowHtml]
        public string SeoDescription { get; set; }

        public DateTime LastModifiedDate { get; set; }

        [AllowHtml]
        public string SeoKeywords { get; set; }

        [Required(ErrorMessage = "Заполните сортировку")]
        public int SortOrder { get; set; }
        [Required(ErrorMessage = "Заполните позиционирование")]
        public int MenuSection { get; set; }

    }
}