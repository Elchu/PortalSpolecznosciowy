namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModelLike : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PostId = c.Int(),
                        CommentId = c.Int(),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.Post", t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Like", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Like", "PostId", "dbo.Post");
            DropForeignKey("dbo.Like", "CommentId", "dbo.Comment");
            DropIndex("dbo.Like", new[] { "CommentId" });
            DropIndex("dbo.Like", new[] { "PostId" });
            DropIndex("dbo.Like", new[] { "UserId" });
            DropTable("dbo.Like");
        }
    }
}
