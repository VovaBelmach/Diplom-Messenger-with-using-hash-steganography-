namespace Messender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dividesellerandreciever : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "SecretImages_Id", "dbo.SecretImages");
            DropIndex("dbo.AspNetUsers", new[] { "SecretImages_Id" });
            AddColumn("dbo.SecretImages", "Reciever_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.SecretImages", "Seller_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.SecretImages", "Reciever_Id");
            CreateIndex("dbo.SecretImages", "Seller_Id");
            AddForeignKey("dbo.SecretImages", "Reciever_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.SecretImages", "Seller_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "SecretImages_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SecretImages_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.SecretImages", "Seller_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SecretImages", "Reciever_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SecretImages", new[] { "Seller_Id" });
            DropIndex("dbo.SecretImages", new[] { "Reciever_Id" });
            DropColumn("dbo.SecretImages", "Seller_Id");
            DropColumn("dbo.SecretImages", "Reciever_Id");
            CreateIndex("dbo.AspNetUsers", "SecretImages_Id");
            AddForeignKey("dbo.AspNetUsers", "SecretImages_Id", "dbo.SecretImages", "Id");
        }
    }
}
