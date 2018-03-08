namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatevf1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VisualFilter", "ParentReportFilterId", "dbo.ReportFilter");
            DropIndex("dbo.VisualFilter", new[] { "ParentReportFilterId" });
            DropColumn("dbo.VisualFilter", "ParentReportFilterId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VisualFilter", "ParentReportFilterId", c => c.Long());
            CreateIndex("dbo.VisualFilter", "ParentReportFilterId");
            AddForeignKey("dbo.VisualFilter", "ParentReportFilterId", "dbo.ReportFilter", "Id");
        }
    }
}
