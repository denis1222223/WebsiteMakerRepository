using CourseProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Environment
{
    public struct SiteInfo
    {
        public Site Site;
        public bool SiteExists;
    }

    public static class SitesRepository
    {
        private static Dictionary<string, SiteInfo> sites = new Dictionary<string, SiteInfo>();

        public static void Add(Site site, bool SiteExists, string userName)
        {
            SiteInfo info = new SiteInfo();
            info.Site = site;
            info.Site.Pages = site.Pages;
            info.SiteExists = SiteExists;
            sites.Add(userName + info.Site.Url, info);
        }

        public static Site GetSite(string id)
        {
            if (id != null && sites.ContainsKey(id))
            {
                return sites[id].Site;
            }
            else
            {
                return null;
            }
        }

        public static bool Exists(string id)
        {
            return sites[id].SiteExists;
        }

        public static void Remove(string id)
        {
            sites.Remove(id);
        }

    }
}