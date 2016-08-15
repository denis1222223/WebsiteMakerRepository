using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Entities;
using System.Data.Entity;
using CourseProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CourseProject.App_Start
{
    public class SiteDdInitializer: DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //Site s1 = new Site { Id = 0, Name = "abw" };
            //Site s2 = new Site { Id = 1, Name = "av" };
            //Site s3 = new Site { Id = 2, Name = "vk" };
            //Site s4 = new Site { Id = 3, Name = "facebook" };

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role1 = new IdentityRole { Name = "Admin" };
            var role2 = new IdentityRole { Name = "User" };

            roleManager.Create(role1);
            roleManager.Create(role2);

            var admin = new ApplicationUser
            {
                Email = "vlados228@gmail.com",
                UserName = "vlados228@gmail.com",
                //Sites = new HashSet<Site> { s3, s4, s1, s2 }
            };
            string password = "123456";
            var result = userManager.Create(admin, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            Site s1 = new Site { Id = 1, Name = "abw", Rating = 2578, Author = admin };
            Site s2 = new Site { Id = 2, Name = "av", Rating = 33333, Author = admin };
            Site s3 = new Site { Id = 3, Name = "vk", Rating = 421, Author = admin };
            Site s4 = new Site { Id = 4, Name = "facebook", Rating = 28, Author = admin };

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