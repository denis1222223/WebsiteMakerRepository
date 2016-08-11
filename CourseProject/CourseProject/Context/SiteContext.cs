using CourseProject.Models.Entities;
using System.Data.Entity;

namespace CourseProject.Models
{
    public class SiteContext : DbContext
    {
        public SiteContext() : base("DefaultConnection")
        {
        }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}