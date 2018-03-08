namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecontract4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContractPriceItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContractId = c.Long(),
                        ItemId = c.Long(),
                        ContractedPrice = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.Contract", t => t.ContractId)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .Index(t => t.ContractId)
                .Index(t => t.ItemId);
            
            AddColumn("dbo.PurchaseOrder", "ContractId", c => c.Long());
            CreateIndex("dbo.PurchaseOrder", "ContractId");
            AddForeignKey("dbo.PurchaseOrder", "ContractId", "dbo.Contract", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrder", "ContractId", "dbo.Contract");
            DropForeignKey("dbo.ContractPriceItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.ContractPriceItem", "ContractId", "dbo.Contract");
            DropIndex("dbo.PurchaseOrder", new[] { "ContractId" });
            DropIndex("dbo.ContractPriceItem", new[] { "ItemId" });
            DropIndex("dbo.ContractPriceItem", new[] { "ContractId" });
            DropColumn("dbo.PurchaseOrder", "ContractId");
            DropTable("dbo.ContractPriceItem");
        }
    }
}
