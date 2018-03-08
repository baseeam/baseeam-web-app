namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addrfq : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestForQuotation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        PurchaseRequestId = c.Long(),
                        RequestorId = c.Long(),
                        ExpectedQuoteDate = c.DateTime(precision: 0),
                        DateRequired = c.DateTime(precision: 0),
                        ShipToAddressId = c.Long(),
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
                .ForeignKey("dbo.PurchaseRequest", t => t.PurchaseRequestId)
                .ForeignKey("dbo.User", t => t.RequestorId)
                .ForeignKey("dbo.Address", t => t.ShipToAddressId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.PurchaseRequestId)
                .Index(t => t.RequestorId)
                .Index(t => t.ShipToAddressId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.RequestForQuotationItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        QuantityRequested = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.RequestForQuotation", t => t.RequestForQuotationId)
                .Index(t => t.RequestForQuotationId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.RequestForQuotationVendor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationId = c.Long(),
                        VendorId = c.Long(),
                        VendorContactName = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactEmail = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactPhone = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorContactFax = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorQuoteNumber = c.String(maxLength: 64, storeType: "nvarchar"),
                        VendorQuoteDate = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.RequestForQuotation", t => t.RequestForQuotationId)
                .ForeignKey("dbo.Company", t => t.VendorId)
                .Index(t => t.RequestForQuotationId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.RequestForQuotationVendorItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestForQuotationVendorId = c.Long(),
                        RequestForQuotationItemId = c.Long(),
                        QuantityQuoted = c.Decimal(precision: 19, scale: 4),
                        UnitPriceQuoted = c.Decimal(precision: 19, scale: 4),
                        SubtotalQuoted = c.Decimal(precision: 19, scale: 4),
                        IsAwarded = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.RequestForQuotationItem", t => t.RequestForQuotationItemId)
                .ForeignKey("dbo.RequestForQuotationVendor", t => t.RequestForQuotationVendorId)
                .Index(t => t.RequestForQuotationVendorId)
                .Index(t => t.RequestForQuotationItemId);
            
            AddColumn("dbo.PurchaseOrder", "RequestForQuotationId", c => c.Long());
            AddColumn("dbo.PurchaseRequest", "AssignmentId", c => c.Long());
            AddColumn("dbo.PurchaseRequest", "CreatedUserId", c => c.Long());
            AddColumn("dbo.PurchaseRequestItem", "QuantityRequested", c => c.Decimal(precision: 19, scale: 4));
            CreateIndex("dbo.PurchaseOrder", "RequestForQuotationId");
            CreateIndex("dbo.PurchaseRequest", "AssignmentId");
            AddForeignKey("dbo.PurchaseRequest", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseOrder", "RequestForQuotationId", "dbo.RequestForQuotation", "Id");
            DropColumn("dbo.PurchaseRequestItem", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseRequestItem", "Quantity", c => c.Decimal(precision: 19, scale: 4));
            DropForeignKey("dbo.PurchaseOrder", "RequestForQuotationId", "dbo.RequestForQuotation");
            DropForeignKey("dbo.RequestForQuotation", "SiteId", "dbo.Site");
            DropForeignKey("dbo.RequestForQuotation", "ShipToAddressId", "dbo.Address");
            DropForeignKey("dbo.RequestForQuotation", "RequestorId", "dbo.User");
            DropForeignKey("dbo.RequestForQuotationVendor", "VendorId", "dbo.Company");
            DropForeignKey("dbo.RequestForQuotationVendorItem", "RequestForQuotationVendorId", "dbo.RequestForQuotationVendor");
            DropForeignKey("dbo.RequestForQuotationVendorItem", "RequestForQuotationItemId", "dbo.RequestForQuotationItem");
            DropForeignKey("dbo.RequestForQuotationVendor", "RequestForQuotationId", "dbo.RequestForQuotation");
            DropForeignKey("dbo.RequestForQuotationItem", "RequestForQuotationId", "dbo.RequestForQuotation");
            DropForeignKey("dbo.RequestForQuotationItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.RequestForQuotation", "PurchaseRequestId", "dbo.PurchaseRequest");
            DropForeignKey("dbo.RequestForQuotation", "AssignmentId", "dbo.Assignment");
            DropForeignKey("dbo.PurchaseRequest", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.RequestForQuotationVendorItem", new[] { "RequestForQuotationItemId" });
            DropIndex("dbo.RequestForQuotationVendorItem", new[] { "RequestForQuotationVendorId" });
            DropIndex("dbo.RequestForQuotationVendor", new[] { "VendorId" });
            DropIndex("dbo.RequestForQuotationVendor", new[] { "RequestForQuotationId" });
            DropIndex("dbo.RequestForQuotationItem", new[] { "ItemId" });
            DropIndex("dbo.RequestForQuotationItem", new[] { "RequestForQuotationId" });
            DropIndex("dbo.RequestForQuotation", new[] { "AssignmentId" });
            DropIndex("dbo.RequestForQuotation", new[] { "ShipToAddressId" });
            DropIndex("dbo.RequestForQuotation", new[] { "RequestorId" });
            DropIndex("dbo.RequestForQuotation", new[] { "PurchaseRequestId" });
            DropIndex("dbo.RequestForQuotation", new[] { "SiteId" });
            DropIndex("dbo.PurchaseRequest", new[] { "AssignmentId" });
            DropIndex("dbo.PurchaseOrder", new[] { "RequestForQuotationId" });
            DropColumn("dbo.PurchaseRequestItem", "QuantityRequested");
            DropColumn("dbo.PurchaseRequest", "CreatedUserId");
            DropColumn("dbo.PurchaseRequest", "AssignmentId");
            DropColumn("dbo.PurchaseOrder", "RequestForQuotationId");
            DropTable("dbo.RequestForQuotationVendorItem");
            DropTable("dbo.RequestForQuotationVendor");
            DropTable("dbo.RequestForQuotationItem");
            DropTable("dbo.RequestForQuotation");
        }
    }
}
