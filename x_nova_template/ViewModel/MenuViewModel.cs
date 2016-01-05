using x_nova_template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{
    public class MenuViewModel
    {
        public IEnumerable<Menu> Menues { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int ctype { get; set; }
    }
}