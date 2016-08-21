using CourseProject.Filters;
using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                try
                {
                    var context = new ApplicationDbContext();
                    var user = context.Users.First(u => u.UserName == HttpContext.User.Identity.Name);
                    ViewBag.ProfilePicture = user.Picture;
                }
                catch { }
            }
        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, writer);
                viewResult.View.Render(viewContext, writer);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return writer.GetStringBuilder().ToString();
            }
        }

    }
}