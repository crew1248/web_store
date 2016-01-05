using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace x_nova_template.Models
{
    [Serializable()]
    public class OrderItem
    {
        public int ID { get; set; }

        [StringLength(300)]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int OrderID { get; set; }
        public float Price { get; set; }
        [StringLength(300)]
        public string Category { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
    }
}