using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{
    public class TreeViewModel
    {
        public string Text { get; set; }
        public List<TreeViewModel> Items { get; set; }
    }
}