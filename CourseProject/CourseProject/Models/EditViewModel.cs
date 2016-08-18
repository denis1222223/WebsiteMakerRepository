using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class EditViewModel
    {
        public string SiteUrl { get; set; }
        public string MenuJson { get; set; }
        public string Theme { get; set; }
        public string ContentJson { get; set; }
    }
}