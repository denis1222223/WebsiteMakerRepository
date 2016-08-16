using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Entities
{
    public class Page
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Template { get; set; }
        public Site Site { get; set; }
    }
}