﻿using System;
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

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role1 = new IdentityRole { Name = "Admin" };
            var role2 = new IdentityRole { Name = "User" };

            roleManager.Create(role1);
            roleManager.Create(role2);

            var admin = new ApplicationUser
            {
                Email = "admin@mail.ru",
                UserName = "admin",
                EmailConfirmed = true,
                Picture = "http://res.cloudinary.com/website-maker/image/upload/v1470265668/default_avatar_ww0y7e.png"
            };
            string password = "denis160797";
            var result = userManager.Create(admin, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            base.Seed(context);
        }
    }
}