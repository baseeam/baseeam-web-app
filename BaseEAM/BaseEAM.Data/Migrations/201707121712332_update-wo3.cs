namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewo3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkOrderMiscCost",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkOrderId = c.Long(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        PlanUnitPrice = c.Decimal(precision: 19, scale: 4),
                        PlanQuantity = c.Decimal(precision: 19, scale: 4),
                        PlanTotal = c.Decimal(precision: 19, scale: 4),
                        ActualUnitPrice = c.Decimal(precision: 19, scale: 4),
                        ActualQuantity = c.Decimal(precision: 19, scale: 4),
                        ActualTotal = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId);
            
            CreateTable(
                "dbo.WorkOrderServiceItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkOrderId = c.Long(),
                        ServiceItemId = c.Long(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        Quantity = c.Decimal(precision: 19, scale: 4),
                        Total = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceItem", t => t.ServiceItemId)
                .ForeignKey("dbo.WorkOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.ServiceItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderServiceItem", "WorkOrderId", "dbo.WorkOrder");
            DropForeignKey("dbo.WorkOrderServiceItem", "ServiceItemId", "dbo.ServiceItem");
            DropForeignKey("dbo.WorkOrderMiscCost", "WorkOrderId", "dbo.WorkOrder");
            DropIndex("dbo.WorkOrderServiceItem", new[] { "ServiceItemId" });
            DropIndex("dbo.WorkOrderServiceItem", new[] { "WorkOrderId" });
            DropIndex("dbo.WorkOrderMiscCost", new[] { "WorkOrderId" });
            DropTable("dbo.WorkOrderServiceItem");
            DropTable("dbo.WorkOrderMiscCost");
        }
    }
}
