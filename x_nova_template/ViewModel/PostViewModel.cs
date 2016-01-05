using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Models;

namespace x_nova_template.ViewModel
{
    public class PostViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}