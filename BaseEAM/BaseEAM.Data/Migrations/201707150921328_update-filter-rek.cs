namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatefilterrek : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportFilter", "ResourceKey", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportFilter", "ResourceKey");
        }
    }
}
