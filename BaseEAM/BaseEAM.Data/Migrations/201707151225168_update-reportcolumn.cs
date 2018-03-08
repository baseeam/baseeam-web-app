namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereportcolumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ReportColumn", "ColumnHeader");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReportColumn", "ColumnHeader", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
    }
}
