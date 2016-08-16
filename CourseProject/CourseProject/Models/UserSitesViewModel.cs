using CourseProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class UserSitesViewModel
    {
        public string UserName { get; set; }
        public bool Author { get; set; }
        public List<Site> Sites { get; set; }
    }
}