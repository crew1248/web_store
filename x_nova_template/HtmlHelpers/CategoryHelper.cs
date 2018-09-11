using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Models;
using x_nova_template.Service.Interface;
using x_nova_template.Service.Repository;

namespace x_nova_template.HtmlHelpers
{
    public static class CategoryHelper
    {
        public static MvcHtmlString GetCats(this HtmlHelper helper, List<Category> catsList)
        {
            StringBuilder str = new StringBuilder();

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("catlist");
            int count = 0;
            foreach (var item in catsList)
            {
                count++;
                string catName = item.CategoryName.IndexOf("&") != 0 ?
                     item.CategoryName.Replace(" & ", "-") :
                     item.CategoryName.Replace(" ", "-");
                TagBuilder liTag = new TagBuilder("li");
                if (count == catsList.Count()) { liTag.AddCssClass("last"); }
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", "/" + catName);

                aTag.AddCssClass("cat-link");
                aTag.SetInnerText(item.CategoryName);
                liTag.InnerHtml += aTag.ToString();
                ulTag.InnerHtml += liTag.ToString();

            }

            str.Append(ulTag.ToString());

            if (catsList.Count() != 0)
            {
                return MvcHtmlString.Create(str.ToString());
            }
            else return MvcHtmlString.Empty;

        }

    }
}