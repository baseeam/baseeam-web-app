namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateac5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutomatedAction", "HoursAfter", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AutomatedAction", "HoursAfter");
        }
    }
}
