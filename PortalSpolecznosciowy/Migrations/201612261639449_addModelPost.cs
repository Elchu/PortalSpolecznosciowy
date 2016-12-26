namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModelPost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false, unicode: false, storeType: "text"),
                        Date = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Post", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Post", new[] { "UserId" });
            DropTable("dbo.Post");
        }
    }
}
