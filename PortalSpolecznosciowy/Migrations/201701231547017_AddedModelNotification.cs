namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        FromWhoId = c.String(maxLength: 128),
                        FullNameFromWho = c.String(),
                        ToWhomId = c.String(maxLength: 128),
                        FullNameToWhom = c.String(),
                        PostId = c.Int(),
                        CommentId = c.Int(),
                        Message = c.String(),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        RecipientHasRead = c.Boolean(nullable: false),
                        SenderInformed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notification");
        }
    }
}
