using CourseProject.Filters;
using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    [Language]
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var context = new ApplicationDbContext();
                var user = context.Users.First(u => u.UserName == HttpContext.User.Identity.Name);
                ViewBag.ProfilePicture = user.Picture;
                ViewBag.Cloudinary = new CloudinaryModel().Cloudinary;
            }
        }

    }
}