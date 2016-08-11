using System;
using System.Collections.Generic;
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
        public ApplicationUser Author { get; set; }
        public string Name { get; set; }
        public int? Rating { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}