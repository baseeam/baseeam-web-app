namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatesyncid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkOrder", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.WorkOrderItem", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.WorkOrderLabor", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.WorkOrderMiscCost", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.WorkOrderServiceItem", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.WorkOrderTask", "SyncId", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkOrderTask", "SyncId", c => c.Guid(nullable: false));
            AlterColumn("dbo.WorkOrderServiceItem", "SyncId", c => c.Guid(nullable: false));
            AlterColumn("dbo.WorkOrderMiscCost", "SyncId", c => c.Guid(nullable: false));
            AlterColumn("dbo.WorkOrderLabor", "SyncId", c => c.Guid(nullable: false));
            AlterColumn("dbo.WorkOrderItem", "SyncId", c => c.Guid(nullable: false));
            AlterColumn("dbo.WorkOrder", "SyncId", c => c.Guid(nullable: false));
        }
    }
}
