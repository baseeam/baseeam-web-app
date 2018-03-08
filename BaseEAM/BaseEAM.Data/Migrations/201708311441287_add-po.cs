namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpo : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.PurchaseRequest", "PurchaseRequestorId", "dbo.User");
            //DropForeignKey("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId", "dbo.UnitOfMeasure");
            //DropIndex("dbo.PurchaseRequestItem", new[] { "PurchaseUnitOfMeasureId" });
            //DropIndex("dbo.PurchaseRequest", new[] { "PurchaseRequestorId" });
            CreateTable(
                "dbo.PurchaseOrderItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseOrderId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        QuantityOrdered = c.Decimal(precision: 19, scale: 4),
                        QuantityReceived = c.Decimal(precision: 19, scale: 4),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        TaxRate = c.Decimal(precision: 19, scale: 4),
                        TaxAmount = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
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
                .ForeignKey("dbo.PurchaseOrder", t => t.PurchaseOrderId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.PurchaseOrder",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        PurchaseRequestId = c.Long(),
                        RequestorId = c.Long(),
                        SupervisorId = c.Long(),
                        ExpectedDeliveryDate = c.DateTime(precision: 0),
                        DateOrdered = c.DateTime(precision: 0),
                        DateRequired = c.DateTime(precision: 0),
                        ShipToAddressId = c.Long(),
                        BillToAddressId = c.Long(),
                        VendorId = c.Long(),
                        PaymentTermId = c.Long(),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Address", t => t.BillToAddressId)
                .ForeignKey("dbo.ValueItem", t => t.PaymentTermId)
                .ForeignKey("dbo.PurchaseRequest", t => t.PurchaseRequestId)
                .ForeignKey("dbo.User", t => t.RequestorId)
                .ForeignKey("dbo.Address", t => t.ShipToAddressId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.User", t => t.SupervisorId)
                .ForeignKey("dbo.Company", t => t.VendorId)
                .Index(t => t.SiteId)
                .Index(t => t.PurchaseRequestId)
                .Index(t => t.RequestorId)
                .Index(t => t.SupervisorId)
                .Index(t => t.ShipToAddressId)
                .Index(t => t.BillToAddressId)
                .Index(t => t.VendorId)
                .Index(t => t.PaymentTermId)
                .Index(t => t.AssignmentId);
            
            DropColumn("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId");
            DropColumn("dbo.PurchaseRequestItem", "PurchaseQuantity");
            DropColumn("dbo.PurchaseRequest", "PurchaseRequestorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseRequest", "PurchaseRequestorId", c => c.Long());
            AddColumn("dbo.PurchaseRequestItem", "PurchaseQuantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId", c => c.Long());
            DropForeignKey("dbo.PurchaseOrderItem", "PurchaseOrderId", "dbo.PurchaseOrder");
            DropForeignKey("dbo.PurchaseOrder", "VendorId", "dbo.Company");
            DropForeignKey("dbo.PurchaseOrder", "SupervisorId", "dbo.User");
            DropForeignKey("dbo.PurchaseOrder", "SiteId", "dbo.Site");
            DropForeignKey("dbo.PurchaseOrder", "ShipToAddressId", "dbo.Address");
            DropForeignKey("dbo.PurchaseOrder", "RequestorId", "dbo.User");
            DropForeignKey("dbo.PurchaseOrder", "PurchaseRequestId", "dbo.PurchaseRequest");
            DropForeignKey("dbo.PurchaseOrder", "PaymentTermId", "dbo.ValueItem");
            DropForeignKey("dbo.PurchaseOrder", "BillToAddressId", "dbo.Address");
            DropForeignKey("dbo.PurchaseOrder", "AssignmentId", "dbo.Assignment");
            DropForeignKey("dbo.PurchaseOrderItem", "ItemId", "dbo.Item");
            DropIndex("dbo.PurchaseOrder", new[] { "AssignmentId" });
            DropIndex("dbo.PurchaseOrder", new[] { "PaymentTermId" });
            DropIndex("dbo.PurchaseOrder", new[] { "VendorId" });
            DropIndex("dbo.PurchaseOrder", new[] { "BillToAddressId" });
            DropIndex("dbo.PurchaseOrder", new[] { "ShipToAddressId" });
            DropIndex("dbo.PurchaseOrder", new[] { "SupervisorId" });
            DropIndex("dbo.PurchaseOrder", new[] { "RequestorId" });
            DropIndex("dbo.PurchaseOrder", new[] { "PurchaseRequestId" });
            DropIndex("dbo.PurchaseOrder", new[] { "SiteId" });
            DropIndex("dbo.PurchaseOrderItem", new[] { "ItemId" });
            DropIndex("dbo.PurchaseOrderItem", new[] { "PurchaseOrderId" });
            DropTable("dbo.PurchaseOrder");
            DropTable("dbo.PurchaseOrderItem");
            CreateIndex("dbo.PurchaseRequest", "PurchaseRequestorId");
            CreateIndex("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId");
            AddForeignKey("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId", "dbo.UnitOfMeasure", "Id");
            AddForeignKey("dbo.PurchaseRequest", "PurchaseRequestorId", "dbo.User", "Id");
        }
    }
}
