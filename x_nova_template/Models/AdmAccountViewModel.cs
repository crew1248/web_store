using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace x_nova_template.Models
{
    public class AdmAccountViewModel
    {
        public int Id { get; set; }
       
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        [Required]
        [Display(Name="Почта")]
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsApproved { get; set; }
    }
}