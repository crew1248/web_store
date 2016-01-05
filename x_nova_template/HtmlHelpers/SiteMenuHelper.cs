using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using x_nova_template.Areas.Admin.Controllers;
using x_nova_template.Models;

namespace x_nova_template.HtmlHelpers
{
    public static class SiteMenuHelper
    {
        public static MvcHtmlString SiteMenuAsUnorderedList(this HtmlHelper helper, List<Menu> siteLinks, bool isFooter = false)
        {
            var CurrentController = (string)helper.ViewContext.RouteData.Values["Controller"];
            var CurrentAction = (string)helper.ViewContext.RouteData.Values["Action"];
            var currPath = helper.ViewContext.HttpContext.Request.Url.AbsolutePath;
            if (siteLinks == null)
                return MvcHtmlString.Empty;
            var topLevelParentId = SiteLinkListHelper.GetTopLevelParentId(siteLinks);
            return MvcHtmlString.Create(buildMenuItems(siteLinks, topLevelParentId, currPath, isFooter));
        }

        private static string buildMenuItems(List<Menu> siteLinks, int parentId, string controllerName, bool isFooter = false)
        {
            if (isFooter)
            {

                var ptag = new TagBuilder("ul");
                ptag.MergeAttribute("class", "tmenu");

                var links = SiteLinkListHelper.GetChildSiteLinks(siteLinks, 0);
                foreach (var siteLink in links)
                {
                    var itag = new TagBuilder("li");
                    var ancht = new TagBuilder("a");

                    ancht.MergeAttribute("href", "/" + siteLink.Url);
                    ancht.SetInnerText(siteLink.Text);
                    itag.InnerHtml = ancht.ToString();

                    ptag.InnerHtml += itag;
                }

                return ptag.ToString();
            }

            var currentUrl = controllerName.Substring(1);
            var parentTag = new TagBuilder("ul");

            if (parentId == 0 && siteLinks.Any(x => x.MenuSection == 1))
            {

                parentTag.MergeAttribute("id", "bmenu");
                parentTag.MergeAttribute("class", "nav navbar-nav bmenu");
            }
            else parentTag.MergeAttribute("class", "nav navbar-nav tmenu");


            var childSiteLinks = SiteLinkListHelper.GetChildSiteLinks(siteLinks, parentId);
            foreach (var siteLink in childSiteLinks.Where(x => x.Id != 14))
            {

                var itemTag = new TagBuilder("li");
                var anchorTag = new TagBuilder("a");
                //var circle = new TagBuilder("i");
                //circle.AddCssClass("icon-circle");
                anchorTag.AddCssClass("i-link");

                if (currentUrl == siteLink.Url)
                {
                    anchorTag.AddCssClass("current");
                    itemTag.AddCssClass("active");
                }

                var newUrl = "/" + siteLink.Url;
                string concatUrl = null;
                if (siteLink.ParentId > 0)
                {
                    //concatUrl = MenuController.GetMenuUrl(siteLink.ParentId) + newUrl;
                    //anchorTag.MergeAttribute("href", concatUrl);
                }
                else
                {
                    if (siteLink.Url == "Home") anchorTag.MergeAttribute("href", "/");
                    else anchorTag.MergeAttribute("href", newUrl);
                }

                anchorTag.SetInnerText(siteLink.Text);
                itemTag.InnerHtml = anchorTag.ToString();
                //if (siteLink.MenuSection == 0) itemTag.InnerHtml += circle.ToString();
                //<ul class="nav navbar-nav">
      //  <li class="active"><a href="#">Ссылка</a></li>
      //  <li><a href="#">Ссылка</a></li>
      //  <li class="dropdown">
      //    <a href="#" class="dropdown-toggle" data-toggle="dropdown">Dropdown <b class="caret"></b></a>
      //    <ul class="dropdown-menu">
      //      <li><a href="#">Действие</a></li>
      //      <li><a href="#">Другое действие</a></li>
      //      <li><a href="#">Что-то еще</a></li>
      //      <li class="divider"></li>
      //      <li><a href="#">Отдельная ссылка</a></li>
      //      <li class="divider"></li>
      //      <li><a href="#">Еще одна отдельная ссылка</a></li>
      //    </ul>
      //  </li>
      //</ul>
                TagBuilder subMenu = new TagBuilder("div"); ;
                if (SiteLinkListHelper.SiteLinkHasChildren(siteLinks, siteLink.Id))
                {
                    //itemTag.InnerHtml += buildMenuItems(siteLinks, siteLink.Id, controllerName); 
                                      
                    var subLinks = new TagBuilder("ul");
                    subLinks.AddCssClass("dropdown-menu");
                    foreach (var subLink in siteLinks.Where(x => x.ParentId == siteLink.Id))
                    {
                        var subItem = new TagBuilder("li");
                        var subAnch = new TagBuilder("a");

                        var subUrl = "/" + subLink.Url;
                        string subConcatUrl = null;
                        subConcatUrl = "/" + MenuController.GetMenuUrl(subLink.ParentId) + subUrl;
                        subAnch.MergeAttribute("href", subConcatUrl);
                        subAnch.SetInnerText(subLink.Text);
                        subItem.AddCssClass("sub-item");
                        subItem.InnerHtml = subAnch.ToString();
                        subLinks.InnerHtml += subItem.ToString();

                    }

                    subMenu.InnerHtml = subLinks.ToString();
                }

                parentTag.InnerHtml += itemTag;
            }
            return parentTag.ToString();
        }

    }
}