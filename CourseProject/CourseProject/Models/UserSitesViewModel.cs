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
        public bool IsAuthor { get; set; }
        public List<Site> Sites { get; set; }
    }
}