using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using x_nova_template.Models;

namespace x_nova_template.Models
{
    public static class SiteLinkListHelper
    {
        public static int GetTopLevelParentId(IEnumerable<Menu> siteLinks)
        {

            return siteLinks.OrderBy(i => i.ParentId).Select(i => i.ParentId).FirstOrDefault();
        }

        public static bool SiteLinkHasChildren(IEnumerable<Menu> siteLinks, int id)
        {
            return siteLinks.Any(i => i.ParentId == id);
        }

        public static IEnumerable<Menu> GetChildSiteLinks(IEnumerable<Menu> siteLinks,
            int parentIdForChildren)
        {
            return siteLinks.Where(i => i.ParentId == parentIdForChildren)
                .OrderBy(i => i.SortOrder).ThenBy(i => i.Text);
        }


    }
}