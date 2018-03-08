namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateactlog2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ActivityLog", "UrlAccessed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityLog", "UrlAccessed", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
    }
}
