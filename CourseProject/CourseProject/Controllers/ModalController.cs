using CourseProject.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    [Language]
    public class ModalController : Controller
    {
       [Route("modal/{modalType}")]
        public ActionResult GetModal(string modalType)
        {
            string veiwName = "Modal" + modalType.Substring(0, 1).ToUpper() + modalType.Remove(0, 1);
            return View(veiwName);
        }
    }
}