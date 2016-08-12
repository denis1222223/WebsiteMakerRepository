using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Entities
{
    public class Site
    {
        public Site()
        {
            Tags = new HashSet<Tag>();
            Comments = new List<Comment>();
        }

        public int Id { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessageResourceName = "CharactersError", ErrorMessageResourceType = typeof(Resource))]
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }
        public int? Rating { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}