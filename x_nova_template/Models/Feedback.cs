using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace x_nova_template.Models
{
    public class Feedback
    {
        [Required(ErrorMessage = "<span class='entypo-alert'></span>Обязательное поле к заполнению")]
        [Display(Name = "Имя пользователя")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "<span class='entypo-alert'></span>Обязательное поле к заполнению")]
        [Display(Name = "Адрес электронной почты")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
                    ErrorMessage = "<span class='entypo-alert'></span>Неверный формат электронной почты")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "<span class='entypo-alert'></span>Обязательное поле к заполнению")]
        [StringLength(250)]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }
       
        [Remote("ValidCaptcha", "Feedback", HttpMethod = "POST",
            ErrorMessage = "<span class='entypo-alert'></span> Ошибка ! Введите код проверки снова.")]
        public string Captcha { get; set; }
    }
}