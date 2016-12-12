namespace PortalSpolecznosciowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredSexAndFullName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.String());
        }
    }
}
