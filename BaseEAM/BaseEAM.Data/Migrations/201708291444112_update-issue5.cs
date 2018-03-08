namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateissue5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issue", "WorkOrderId", c => c.Long());
            CreateIndex("dbo.Issue", "WorkOrderId");
            AddForeignKey("dbo.Issue", "WorkOrderId", "dbo.WorkOrder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Issue", "WorkOrderId", "dbo.WorkOrder");
            DropIndex("dbo.Issue", new[] { "WorkOrderId" });
            DropColumn("dbo.Issue", "WorkOrderId");
        }
    }
}
