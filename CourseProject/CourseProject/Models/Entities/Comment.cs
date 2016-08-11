using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Site Site{ get; set; }
    }
}