﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class EditViewModel
    {
        public string SiteUrl { get; set; }
        public string MenuHtml { get; set; }
        public string Theme { get; set; }
        public string ContentHtml { get; set; }
    }
}