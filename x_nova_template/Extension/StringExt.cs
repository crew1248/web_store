﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace x_nova_template.Extension
{
    public static class StringExt
    {
        
        public  static string ToCutedUsername(string str) {
            
            var name = Regex.Replace(str, @"@\w+.\w+", " ");
            return name;
        }

    }
}