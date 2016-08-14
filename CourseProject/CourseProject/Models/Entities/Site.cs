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
            Pages = new List<Page>();
            Comments = new List<Comment>();
        }

        public int Id { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}