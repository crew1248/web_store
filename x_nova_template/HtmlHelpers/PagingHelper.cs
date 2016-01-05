using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using x_nova_template.ViewModel;

namespace x_nova_template.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pageingInfo, Func<int, string> pageUrl)
        {
            StringBuilder str = new StringBuilder();


            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = pageingInfo.CurrentPage;
            var totalPages =pageingInfo.TotalPage;
            

            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            for (int i = 1; i <= totalPages; i++)
            {
                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {
                TagBuilder tag = new TagBuilder("a");
                    
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();


                if (i == currentPage) 
                    tag.AddCssClass("current-page k-primary k-button");
                else 
                    tag.AddCssClass("k-button");

                str.Append(tag.ToString());
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            str.Append("...");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            str.Append("...");
                            bThreeDots2 = true;
                        }
                    }
                }
            }
            if (pageingInfo.TotalPage > 1)
            {
                return MvcHtmlString.Create(str.ToString());
            }
            else return MvcHtmlString.Empty;
        }
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pageingInfo, Func<int,string, string> pageUrl)
        {
            StringBuilder str = new StringBuilder();


            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = pageingInfo.CurrentPage;
            var totalPages = pageingInfo.TotalPage;
            var sort = pageingInfo.Sort;

            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            for (int i = 1; i <= totalPages; i++)
            {
                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {
                    TagBuilder tag = new TagBuilder("a");

                    tag.MergeAttribute("href", pageUrl(i,sort));
                    tag.InnerHtml = i.ToString();


                    if (i == currentPage)
                    {
                        tag.AddCssClass("current-page");
                        tag.AddCssClass("k-primary");
                    }
                    else tag.AddCssClass("k-button");
                    str.Append(tag.ToString());
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            str.Append("...");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            str.Append("...");
                            bThreeDots2 = true;
                        }
                    }
                }
            }
            if (pageingInfo.TotalPage > 1)
            {
                return MvcHtmlString.Create(str.ToString());
            }
            else return MvcHtmlString.Empty;
        }

        public static MvcHtmlString AjaxPageLinks(this AjaxHelper html, PagingInfo pageingInfo)
        {
            StringBuilder str = new StringBuilder();

            
            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = pageingInfo.CurrentPage;
            var totalPages = pageingInfo.TotalPage;


            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            for (int i = 1; i <= totalPages; i++)
            {
                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", "javascript:void()");
                    tag.InnerHtml = i.ToString();
                    tag.MergeAttribute("data-service", pageingInfo.Service);
                    tag.MergeAttribute("data-page", i.ToString());
                    if (pageingInfo.CatId != null)
                    {
                        tag.MergeAttribute("data-catid", pageingInfo.CatId.ToString());
                    }
                    tag.AddCssClass("page-link k-button");

                    if (i == currentPage)
                        tag.AddCssClass("current-page k-button k-primary");

                    str.Append(tag.ToString());
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            str.Append("...");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            str.Append("...");
                            bThreeDots2 = true;
                        }
                    }
                }
            }
            if (pageingInfo.TotalPage > 1)
            {
                return MvcHtmlString.Create(str.ToString());
            }
            else return MvcHtmlString.Empty;
        }
    }



}