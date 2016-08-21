using CourseProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace CourseProject.Environment
{
    public struct SiteInfo
    {
        public Site Site { get; set; }
        public bool SiteExists { get; set; }
        public DateTime LastAccessTime { get; set; }
    }

    public static class SitesRepository
    {
        private static int checkPeriod = 1200000;

        private static Dictionary<string, SiteInfo> sites = new Dictionary<string, SiteInfo>();

        public static void Add(Site site, bool siteExists, string userName)
        {
            SiteInfo info = FillSiteInfo(site, siteExists, userName);
            try
            {
                if (!sites.ContainsKey(userName + site.Url))
                {
                    sites.Add(userName + site.Url, info);
                }
            }
            catch { }         
        }

        private static SiteInfo FillSiteInfo(Site site, bool siteExists, string userName)
        {
            SiteInfo info = new SiteInfo();
            info.Site = site;
            info.Site.Pages = site.Pages;
            info.Site.Comments = site.Comments;
            info.SiteExists = siteExists;
            info.LastAccessTime = DateTime.UtcNow;
            return info;
        }

        internal static void InitializeRepositoryTimer()
        {
            
            TimerCallback CheckSitesCallback = new TimerCallback(CheckSites);
            Timer timer = new Timer( CheckSitesCallback, null, 0, checkPeriod);
        }

        private static void CheckSites(object state)
        {
            foreach(var info in sites)
            {
                if((DateTime.UtcNow - info.Value.LastAccessTime).TotalMilliseconds > checkPeriod)
                {
                    new Controllers.SitesController().UpdateSite(info.Value.Site);
                    sites.Remove(info.Key);
                }
            }
        }

        public static Site GetSite(string id)
        {
            if (id != null && sites.ContainsKey(id))
            {
                SiteInfo info = sites[id];
                info.LastAccessTime = DateTime.UtcNow;
                sites[id] = info;
                return sites[id].Site;
            }
            else
            {
                return null;
            }
        }

        public static bool Exists(string id)
        {
            SiteInfo info = sites[id];
            info.LastAccessTime = DateTime.UtcNow;
            return sites[id].SiteExists;
        }

        public static void Remove(string id)
        {
            if (id != null && sites.ContainsKey(id))
            {
                sites.Remove(id);
            }
        }

    }
}