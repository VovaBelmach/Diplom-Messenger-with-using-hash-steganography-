namespace Messender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingURltoimages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "UrlToImage", c => c.String());
            DropColumn("dbo.Images", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Content", c => c.Binary());
            DropColumn("dbo.Images", "UrlToImage");
        }
    }
}
