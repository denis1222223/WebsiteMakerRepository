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
        private static Dictionary<int, SiteInfo> sites = new Dictionary<int, SiteInfo>();

        public static void Add(Site site, bool SiteExists)
        {
            SiteInfo info = new SiteInfo();
            info.Site = site;
            info.SiteExists = SiteExists;
            sites.Add(site.Id, info);
        }

        public static Site GetSite(int? id)
        {
            if(id != null || sites.ContainsKey((int)id))
            {
                return sites[(int)id].Site;
            }
           else
            {
                return null;
            }
        }

        public static bool Exists(int id)
        {
            return sites[id].SiteExists;
        }

        public static void Remove(int id)
        {
            sites.Remove(id);
        }

    }
}