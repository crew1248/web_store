using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace x_nova_template.ViewModel
{
    public class FilesViewModel
    {
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<FileInfo> Files { get; set; }
    }
}