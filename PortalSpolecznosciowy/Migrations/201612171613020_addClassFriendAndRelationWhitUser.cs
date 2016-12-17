namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addClassFriendAndRelationWhitUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserFriendId = c.String(maxLength: 128),
                        Accepted = c.Boolean(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId, unique: true)
                .Index(t => t.UserFriendId, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friend", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Friend", new[] { "UserFriendId" });
            DropIndex("dbo.Friend", new[] { "UserId" });
            DropTable("dbo.Friend");
        }
    }
}
