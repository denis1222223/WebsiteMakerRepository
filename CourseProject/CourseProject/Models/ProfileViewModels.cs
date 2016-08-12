using CourseProject.Environment;
using CourseProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class ProfileViewModel
    {
        public List<Site> Sites { get; set; }
        public ApplicationUser User { get; set; }
    }
}