namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseRequestItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseRequestId = c.Long(),
                        Sequence = c.Int(),
                        ItemId = c.Long(),
                        PurchaseUnitOfMeasureId = c.Long(),
                        PurchaseQuantity = c.Decimal(precision: 19, scale: 4),
                        Quantity = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.PurchaseRequest", t => t.PurchaseRequestId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.PurchaseUnitOfMeasureId)
                .Index(t => t.PurchaseRequestId)
                .Index(t => t.ItemId)
                .Index(t => t.PurchaseUnitOfMeasureId);
            
            CreateTable(
                "dbo.PurchaseRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        SiteId = c.Long(),
                        PurchaseRequestorId = c.Long(),
                        DateRequired = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.User", t => t.PurchaseRequestorId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.PurchaseRequestorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseRequestItem", "PurchaseUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.PurchaseRequestItem", "PurchaseRequestId", "dbo.PurchaseRequest");
            DropForeignKey("dbo.PurchaseRequest", "SiteId", "dbo.Site");
            DropForeignKey("dbo.PurchaseRequest", "PurchaseRequestorId", "dbo.User");
            DropForeignKey("dbo.PurchaseRequestItem", "ItemId", "dbo.Item");
            DropIndex("dbo.PurchaseRequest", new[] { "PurchaseRequestorId" });
            DropIndex("dbo.PurchaseRequest", new[] { "SiteId" });
            DropIndex("dbo.PurchaseRequestItem", new[] { "PurchaseUnitOfMeasureId" });
            DropIndex("dbo.PurchaseRequestItem", new[] { "ItemId" });
            DropIndex("dbo.PurchaseRequestItem", new[] { "PurchaseRequestId" });
            DropTable("dbo.PurchaseRequest");
            DropTable("dbo.PurchaseRequestItem");
        }
    }
}
