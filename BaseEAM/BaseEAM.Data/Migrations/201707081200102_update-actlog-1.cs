namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateactlog1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ActivityLog", "UserHostAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityLog", "UserHostAddress", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
    }
}
