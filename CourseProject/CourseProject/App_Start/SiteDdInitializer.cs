using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Entities;
using System.Data.Entity;
using CourseProject.Models;

namespace CourseProject.App_Start
{
    public class SiteDdInitializer: DropCreateDatabaseAlways<SiteContext>
    {
        protected override void Seed(SiteContext context)
        {
            Site s1 = new Site { Id = 1, Name = "abw", Rating = 2578 };
            Site s2 = new Site { Id = 2, Name = "av", Rating = 33333 };
            Site s3 = new Site { Id = 3, Name = "vk", Rating = 421 };
            Site s4 = new Site { Id = 4, Name = "facebook", Rating = 28 };

            context.Sites.Add(s1);
            context.Sites.Add(s2);
            context.Sites.Add(s3);
            context.Sites.Add(s4);

            Tag c1 = new Tag
            {
                Id = 1,
                Name = "like",
                Sites = new HashSet<Site>() { s1, s2, s3 }
            };
            Tag c2 = new Tag
            {
                Id = 2,
                Name = "girls",
                Sites = new HashSet<Site> { s2, s4 }
            };
            Tag c3 = new Tag
            {
                Id = 3,
                Name = "love",
                Sites = new HashSet<Site> { s3, s4, s1 }
            };

            context.Tags.Add(c1);
            context.Tags.Add(c2);
            context.Tags.Add(c3);

            base.Seed(context);
        }
    }
}