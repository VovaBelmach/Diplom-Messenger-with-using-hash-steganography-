namespace Messender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletingHAshes : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Hashes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Hashes",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Url = c.String(),
                        Hash = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
    }
}
