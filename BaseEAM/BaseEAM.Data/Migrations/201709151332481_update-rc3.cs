namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updaterc3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportColumn", "DisplayOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportColumn", "DisplayOrder");
        }
    }
}
