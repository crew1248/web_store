using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using x_nova_template.Providers;

namespace x_nova_template.Extension
{
    public static class FileFormat
    {
        public static string ToFileSize(this long l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }
    }
}