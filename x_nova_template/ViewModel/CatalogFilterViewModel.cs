using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{

    public class PriceFilter
    {
        public int price { get; set; }
    }
    public class SizeFilter
    {
        public string size { get; set; }
    }

    public class CatFilter
    {
        public string catId { get; set; }
    }
}