﻿using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class HomeController : BaseController
    {       
        public ActionResult Index()
        {
            return View();
        }

    }
}