/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addstoreitem3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StoreId = c.Long(),
                        ItemId = c.Long(),
                        StockType = c.Int(),
                        LotType = c.Int(),
                        CostingType = c.Int(),
                        StandardCostingUnitPrice = c.Decimal(precision: 19, scale: 4),
                        SafetyStock = c.Decimal(precision: 19, scale: 4),
                        ReorderPoint = c.Decimal(precision: 19, scale: 4),
                        EconomicOrderQuantity = c.Decimal(precision: 19, scale: 4),
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
                .Index(t => t.StoreId)
                .Index(t => t.ItemId);
            
            DropColumn("dbo.Item", "ItemStockType");
            DropColumn("dbo.Item", "ItemLotType");
            DropColumn("dbo.Item", "ItemCostingType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "ItemCostingType", c => c.Int());
            AddColumn("dbo.Item", "ItemLotType", c => c.Int());
            AddColumn("dbo.Item", "ItemStockType", c => c.Int());
            DropForeignKey("dbo.StoreItem", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreItem", "ItemId", "dbo.Item");
            DropIndex("dbo.StoreItem", new[] { "ItemId" });
            DropIndex("dbo.StoreItem", new[] { "StoreId" });
            DropTable("dbo.StoreItem");
        }
    }
}
