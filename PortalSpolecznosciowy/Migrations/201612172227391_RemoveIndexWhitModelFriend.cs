namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIndexWhitModelFriend : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Friend", new[] { "UserId" });
            DropIndex("dbo.Friend", new[] { "UserFriendId" });
            CreateIndex("dbo.Friend", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Friend", new[] { "UserId" });
            CreateIndex("dbo.Friend", "UserFriendId", unique: true);
            CreateIndex("dbo.Friend", "UserId", unique: true);
        }
    }
}
