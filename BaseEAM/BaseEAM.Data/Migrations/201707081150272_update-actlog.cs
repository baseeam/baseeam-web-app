namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateactlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActivityLog", "UserHostAddress", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.ActivityLog", "UrlAccessed", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.ActivityLog", "Comment", c => c.String(maxLength: 256, storeType: "nvarchar"));
            AlterColumn("dbo.ActivityLogType", "Name", c => c.String(maxLength: 256, storeType: "nvarchar"));
            DropColumn("dbo.ActivityLog", "CreatedOnUtc");
            DropColumn("dbo.ActivityLogType", "SystemKeyword");
            DropColumn("dbo.ActivityLogType", "Enabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityLogType", "Enabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ActivityLogType", "SystemKeyword", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            AddColumn("dbo.ActivityLog", "CreatedOnUtc", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.ActivityLogType", "Name", c => c.String(nullable: false, maxLength: 200, storeType: "nvarchar"));
            AlterColumn("dbo.ActivityLog", "Comment", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.ActivityLog", "UrlAccessed");
            DropColumn("dbo.ActivityLog", "UserHostAddress");
        }
    }
}
