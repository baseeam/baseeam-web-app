/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addtransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransferItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransferId = c.Long(),
                        FromStoreLocatorId = c.Long(),
                        ToStoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        TransferQuantity = c.Decimal(precision: 19, scale: 4),
                        TransferUnitOfMeasureId = c.Long(),
                        TransferCost = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreLocator", t => t.FromStoreLocatorId)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.StoreLocator", t => t.ToStoreLocatorId)
                .ForeignKey("dbo.Transfer", t => t.TransferId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.TransferUnitOfMeasureId)
                .Index(t => t.TransferId)
                .Index(t => t.FromStoreLocatorId)
                .Index(t => t.ToStoreLocatorId)
                .Index(t => t.ItemId)
                .Index(t => t.TransferUnitOfMeasureId);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        TransferDate = c.DateTime(precision: 0),
                        FromSiteId = c.Long(),
                        FromStoreId = c.Long(),
                        ToSiteId = c.Long(),
                        ToStoreId = c.Long(),
                        IsApproved = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.FromSiteId)
                .ForeignKey("dbo.Store", t => t.FromStoreId)
                .ForeignKey("dbo.Site", t => t.ToSiteId)
                .ForeignKey("dbo.Store", t => t.ToStoreId)
                .Index(t => t.FromSiteId)
                .Index(t => t.FromStoreId)
                .Index(t => t.ToSiteId)
                .Index(t => t.ToStoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransferItem", "TransferUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.TransferItem", "TransferId", "dbo.Transfer");
            DropForeignKey("dbo.Transfer", "ToStoreId", "dbo.Store");
            DropForeignKey("dbo.Transfer", "ToSiteId", "dbo.Site");
            DropForeignKey("dbo.Transfer", "FromStoreId", "dbo.Store");
            DropForeignKey("dbo.Transfer", "FromSiteId", "dbo.Site");
            DropForeignKey("dbo.TransferItem", "ToStoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.TransferItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.TransferItem", "FromStoreLocatorId", "dbo.StoreLocator");
            DropIndex("dbo.Transfer", new[] { "ToStoreId" });
            DropIndex("dbo.Transfer", new[] { "ToSiteId" });
            DropIndex("dbo.Transfer", new[] { "FromStoreId" });
            DropIndex("dbo.Transfer", new[] { "FromSiteId" });
            DropIndex("dbo.TransferItem", new[] { "TransferUnitOfMeasureId" });
            DropIndex("dbo.TransferItem", new[] { "ItemId" });
            DropIndex("dbo.TransferItem", new[] { "ToStoreLocatorId" });
            DropIndex("dbo.TransferItem", new[] { "FromStoreLocatorId" });
            DropIndex("dbo.TransferItem", new[] { "TransferId" });
            DropTable("dbo.Transfer");
            DropTable("dbo.TransferItem");
        }
    }
}
