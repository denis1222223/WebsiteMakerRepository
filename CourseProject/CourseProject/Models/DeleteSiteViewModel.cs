using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class DeleteSiteViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(Name = "URL")]
        public string Url { get; set; }

        [Display(Name = "Rating", ResourceType = typeof(Resource))]
        public int? Rating { get; set; }
    }
}