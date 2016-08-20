using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace CourseProject.Models
{
    public class DetailsViewModel
    {
        public bool horizontalMenu;
        public bool verticalMenu;
        public string SiteName { get; set; }
        public string MenuJson { get; set; }
        public string Theme { get; set; }
        public string ContentJson { get; set; }
        public string Template { get; set; }
        public bool IsEdit { get; set; }
    }
}