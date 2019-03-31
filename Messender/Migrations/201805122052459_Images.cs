namespace Messender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.Binary(),
                        SecretImages_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecretImages", t => t.SecretImages_Id)
                .Index(t => t.SecretImages_Id);
            
            DropColumn("dbo.SecretImages", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SecretImages", "Image", c => c.Binary());
            DropForeignKey("dbo.Images", "SecretImages_Id", "dbo.SecretImages");
            DropIndex("dbo.Images", new[] { "SecretImages_Id" });
            DropTable("dbo.Images");
        }
    }
}
