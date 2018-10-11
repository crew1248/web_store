using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    public class Config
    {
        [HiddenInput(DisplayValue = false)]
        public int ConfigID { get; set; }


        [Display(Name = "Название сайта")]
        [StringLength(150)]
        public string SiteName { get; set; }

        [Display(Name = "Robots.txt")]
        [StringLength(500)]
        public string Robots { get; set; }

        [Display(Name = "Адрес сайта")]
        [StringLength(100)]
        [Url(ErrorMessage = "введите полный адрес страницы")]
        public string SiteAddress { get; set; }

        [Display(Name = "Описание сайта")]
        [StringLength(250)]
        public string Description { get; set; }

        [Display(Name = "Ключевые слова")]
        [StringLength(150)]
        public string Keywords { get; set; }

        [Display(Name = "Email")]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "некоректная почта")]
        public string Email { get; set; }

        [Display(Name = "Отключить сайт")]
        public bool SelectedIsOnlineID { get; set; }

        [Display(Name = "Сообщение при неработающем сайте")]
        public string OfflineMessage { get; set; }



    }


}