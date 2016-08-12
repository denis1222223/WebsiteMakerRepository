namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SiteMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Site_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Sites", t => t.Site_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Site_Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Rating = c.Int(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagSites",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Site_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Site_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.Site_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Site_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagSites", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.TagSites", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Comments", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.Sites", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TagSites", new[] { "Site_Id" });
            DropIndex("dbo.TagSites", new[] { "Tag_Id" });
            DropIndex("dbo.Sites", new[] { "Author_Id" });
            DropIndex("dbo.Comments", new[] { "Site_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropTable("dbo.TagSites");
            DropTable("dbo.Tags");
            DropTable("dbo.Sites");
            DropTable("dbo.Comments");
        }
    }
}
