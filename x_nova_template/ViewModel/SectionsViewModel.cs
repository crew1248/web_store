using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;
using x_nova_template.ViewModel;

namespace x_nova_template.ViewModel
{
    public class SectionsViewModel
    {
        public IEnumerable<StaticSection> StaticSections { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}