namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatevf2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisualFilter", "ParentVisualFilterId", c => c.Long());
            CreateIndex("dbo.VisualFilter", "ParentVisualFilterId");
            AddForeignKey("dbo.VisualFilter", "ParentVisualFilterId", "dbo.ReportFilter", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisualFilter", "ParentVisualFilterId", "dbo.ReportFilter");
            DropIndex("dbo.VisualFilter", new[] { "ParentVisualFilterId" });
            DropColumn("dbo.VisualFilter", "ParentVisualFilterId");
        }
    }
}
