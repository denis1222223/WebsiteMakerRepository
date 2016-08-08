using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseProject.Environment;

namespace CourseProject.Controllers
{
    public class SiteController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Create()
        {
            String result = "";
            foreach (var tag in TagsParser.Parse(Request.Form["Tags"]))
            {
                result += tag + " ";
            }
            return "Спасибо, " + result + ", за покупку!";
        }
    }
}