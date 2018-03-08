namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addsyncid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrder", "SyncId", c => c.Guid(nullable: false));
            AddColumn("dbo.WorkOrderItem", "SyncId", c => c.Guid(nullable: false));
            AddColumn("dbo.WorkOrderLabor", "SyncId", c => c.Guid(nullable: false));
            AddColumn("dbo.WorkOrderMiscCost", "SyncId", c => c.Guid(nullable: false));
            AddColumn("dbo.WorkOrderServiceItem", "SyncId", c => c.Guid(nullable: false));
            AddColumn("dbo.WorkOrderTask", "SyncId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrderTask", "SyncId");
            DropColumn("dbo.WorkOrderServiceItem", "SyncId");
            DropColumn("dbo.WorkOrderMiscCost", "SyncId");
            DropColumn("dbo.WorkOrderLabor", "SyncId");
            DropColumn("dbo.WorkOrderItem", "SyncId");
            DropColumn("dbo.WorkOrder", "SyncId");
        }
    }
}
