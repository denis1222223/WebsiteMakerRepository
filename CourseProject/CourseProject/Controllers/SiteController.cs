using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class SiteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Create()
        {
            return "Спасибо, " + Request.Form.Keys + ", за покупку!";
        }
    }
}