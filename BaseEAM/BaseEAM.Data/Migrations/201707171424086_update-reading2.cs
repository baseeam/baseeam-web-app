namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereading2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reading", "WorkOrderId", c => c.Long());
            CreateIndex("dbo.Reading", "WorkOrderId");
            AddForeignKey("dbo.Reading", "WorkOrderId", "dbo.WorkOrder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reading", "WorkOrderId", "dbo.WorkOrder");
            DropIndex("dbo.Reading", new[] { "WorkOrderId" });
            DropColumn("dbo.Reading", "WorkOrderId");
        }
    }
}
