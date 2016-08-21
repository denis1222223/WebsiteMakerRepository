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
            RatedUsers = new List<string>();
        }

        public int Id { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Theme { get; set; }
        public string MenuJson { get; set; }
        public int Rating { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public bool AllowComments { get; set; }
        public bool AllowRating { get; set; }
        public List<string> RatedUsers { get; set; }
    }
}