using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Models.Entities
{
    public class Comment
    {
        public Comment(ApplicationUser author, string text, Site site)
        {
            Author = author;
            Text = text;
            Site = site;
            Date = DateTime.Now;
        }
        public Comment() { }
        public int Id { get; set; }
        [Required]
        public ApplicationUser Author { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public Site Site{ get; set; }
    }
}