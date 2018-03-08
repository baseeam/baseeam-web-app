namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepo6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrderMiscCost",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseOrderId = c.Long(),
                        POMiscCostTypeId = c.Long(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Amount = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.ValueItem", t => t.POMiscCostTypeId)
                .ForeignKey("dbo.PurchaseOrder", t => t.PurchaseOrderId)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.POMiscCostTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrderMiscCost", "PurchaseOrderId", "dbo.PurchaseOrder");
            DropForeignKey("dbo.PurchaseOrderMiscCost", "POMiscCostTypeId", "dbo.ValueItem");
            DropIndex("dbo.PurchaseOrderMiscCost", new[] { "POMiscCostTypeId" });
            DropIndex("dbo.PurchaseOrderMiscCost", new[] { "PurchaseOrderId" });
            DropTable("dbo.PurchaseOrderMiscCost");
        }
    }
}
