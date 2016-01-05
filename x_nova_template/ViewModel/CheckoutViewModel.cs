using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace x_nova_template.ViewModel
{
    [Serializable]
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "заполните имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "заполните фамилию")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "заполните адрес доставки")]
        public string Address { get; set; }
        [Required(ErrorMessage = "заполните номер телефона")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "заполните почту")]
        [EmailAddress(ErrorMessage = "введите правильный формат почты")]
        public string Email { get; set; }

        public string Payment { get; set; }
        public string Delivery { get; set; }


        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Comment { get; set; }

        public string Name { get; set; }

        public IEnumerable<SelectListItem> DeliveryList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Text="В радиусе МКАД",Value="В радиусе МКАД"},
                    new SelectListItem{Text="За МКАД",Value="За МКАД"}
                };
            }
        }
        public IEnumerable<SelectListItem> PaymentList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Text="Яндекс.Деньги",Value="Яндекс.Деньги"},
                    new SelectListItem{Text="WebMoney",Value="WebMoney"},
                    new SelectListItem{Text="Visa/Mastercard",Value="Visa/Mastercard"},
                    new SelectListItem{Text="Оплата наличными",Value="Оплата наличными"}
                };
            }
        }


    }
    [Serializable]
    public class Checkout_Delivery
    {
        public string Delivery { get; set; }
    }
    [Serializable]
    public class Checkout_Payment
    {
        public string Payment { get; set; }
    }
}