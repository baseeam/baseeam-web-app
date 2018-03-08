namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addwoitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkOrderItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkOrderId = c.Long(),
                        StoreId = c.Long(),
                        ItemId = c.Long(),
                        StoreLocatorId = c.Long(),
                        PlanUnitPrice = c.Decimal(precision: 19, scale: 4),
                        PlanQuantity = c.Decimal(precision: 19, scale: 4),
                        ActualUnitPrice = c.Decimal(precision: 19, scale: 4),
                        ActualQuantity = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .ForeignKey("dbo.WorkOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.StoreId)
                .Index(t => t.ItemId)
                .Index(t => t.StoreLocatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderItem", "WorkOrderId", "dbo.WorkOrder");
            DropForeignKey("dbo.WorkOrderItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.WorkOrderItem", "StoreId", "dbo.Store");
            DropForeignKey("dbo.WorkOrderItem", "ItemId", "dbo.Item");
            DropIndex("dbo.WorkOrderItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.WorkOrderItem", new[] { "ItemId" });
            DropIndex("dbo.WorkOrderItem", new[] { "StoreId" });
            DropIndex("dbo.WorkOrderItem", new[] { "WorkOrderId" });
            DropTable("dbo.WorkOrderItem");
        }
    }
}
