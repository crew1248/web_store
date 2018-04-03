using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Service { get; set; }
        public int CatId { get; set; }
        public string Sort { get; set; }
        public DirectoryInfo Dir { get; set; }

        public int TotalPage
        {
            get { return (int)Math.Ceiling((double)TotalItems / (double)ItemsPerPage); }
        }
    }
}