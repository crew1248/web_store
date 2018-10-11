using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x_nova_template.Extension
{
    public static class LinqTest
    {
        public static string ToFullNames(this string firstName)
        {
            return firstName.ToUpper();
        }

    }
}