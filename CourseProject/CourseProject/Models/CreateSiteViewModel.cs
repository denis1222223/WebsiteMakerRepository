﻿using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class CreateSiteViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessageResourceName = "CharactersError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[a-z0-9_-]+$", ErrorMessageResourceName = "UrlCharactersError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "URL")]
        public string Url { get; set; }
    }
}