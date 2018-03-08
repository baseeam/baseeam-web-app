/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addstoreitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreLocatorItemLog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StoreLocatorItemId = c.Long(),
                        StoreId = c.Long(),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        QuantityChanged = c.Decimal(precision: 19, scale: 4),
                        CostChanged = c.Decimal(precision: 19, scale: 4),
                        TransactionType = c.String(maxLength: 64, storeType: "nvarchar"),
                        TransactionId = c.Long(),
                        TransactionNumber = c.String(maxLength: 64, storeType: "nvarchar"),
                        TransactionDate = c.DateTime(precision: 0),
                        TransactionItemId = c.Long(),
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
                .ForeignKey("dbo.StoreLocatorItem", t => t.StoreLocatorItemId)
                .Index(t => t.StoreLocatorItemId)
                .Index(t => t.StoreId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.StoreLocatorItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StoreId = c.Long(),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        Quantity = c.Decimal(precision: 19, scale: 4),
                        Cost = c.Decimal(precision: 19, scale: 4),
                        BatchDate = c.DateTime(precision: 0),
                        LotNumber = c.String(maxLength: 64, storeType: "nvarchar"),
                        ExpiryDate = c.DateTime(precision: 0),
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
                .Index(t => t.StoreId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.IssueItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IssueId = c.Long(),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        IssueQuantity = c.Decimal(precision: 19, scale: 4),
                        IssueUnitOfMeasureId = c.Long(),
                        IssueCost = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issue", t => t.IssueId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.IssueUnitOfMeasureId)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .Index(t => t.IssueId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId)
                .Index(t => t.IssueUnitOfMeasureId);
            
            CreateTable(
                "dbo.Issue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        IssueDate = c.DateTime(precision: 0),
                        SiteId = c.Long(),
                        StoreId = c.Long(),
                        IsApproved = c.Boolean(nullable: false),
                        UserId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.SiteId)
                .Index(t => t.StoreId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ReceiptItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReceiptId = c.Long(),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        Quantity = c.Decimal(precision: 19, scale: 4),
                        Cost = c.Decimal(precision: 19, scale: 4),
                        LotNumber = c.String(maxLength: 64, storeType: "nvarchar"),
                        ExpiryDate = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.Receipt", t => t.ReceiptId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .Index(t => t.ReceiptId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Receipt",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        ReceiptDate = c.DateTime(precision: 0),
                        SiteId = c.Long(),
                        StoreId = c.Long(),
                        IsApproved = c.Boolean(nullable: false),
                        UserId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.SiteId)
                .Index(t => t.StoreId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiptItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.ReceiptItem", "ReceiptId", "dbo.Receipt");
            DropForeignKey("dbo.Receipt", "UserId", "dbo.User");
            DropForeignKey("dbo.Receipt", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Receipt", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ReceiptItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.IssueItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.IssueItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.IssueItem", "IssueUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.IssueItem", "IssueId", "dbo.Issue");
            DropForeignKey("dbo.Issue", "UserId", "dbo.User");
            DropForeignKey("dbo.Issue", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Issue", "SiteId", "dbo.Site");
            DropForeignKey("dbo.StoreLocatorItemLog", "StoreLocatorItemId", "dbo.StoreLocatorItem");
            DropForeignKey("dbo.StoreLocatorItemLog", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.StoreLocatorItemLog", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLocatorItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.StoreLocatorItem", "StoreId", "dbo.Store");
            DropForeignKey("dbo.StoreLocatorItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.StoreLocatorItemLog", "ItemId", "dbo.Item");
            DropIndex("dbo.Receipt", new[] { "UserId" });
            DropIndex("dbo.Receipt", new[] { "StoreId" });
            DropIndex("dbo.Receipt", new[] { "SiteId" });
            DropIndex("dbo.ReceiptItem", new[] { "ItemId" });
            DropIndex("dbo.ReceiptItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.ReceiptItem", new[] { "ReceiptId" });
            DropIndex("dbo.Issue", new[] { "UserId" });
            DropIndex("dbo.Issue", new[] { "StoreId" });
            DropIndex("dbo.Issue", new[] { "SiteId" });
            DropIndex("dbo.IssueItem", new[] { "IssueUnitOfMeasureId" });
            DropIndex("dbo.IssueItem", new[] { "ItemId" });
            DropIndex("dbo.IssueItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.IssueItem", new[] { "IssueId" });
            DropIndex("dbo.StoreLocatorItem", new[] { "ItemId" });
            DropIndex("dbo.StoreLocatorItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.StoreLocatorItem", new[] { "StoreId" });
            DropIndex("dbo.StoreLocatorItemLog", new[] { "ItemId" });
            DropIndex("dbo.StoreLocatorItemLog", new[] { "StoreLocatorId" });
            DropIndex("dbo.StoreLocatorItemLog", new[] { "StoreId" });
            DropIndex("dbo.StoreLocatorItemLog", new[] { "StoreLocatorItemId" });
            DropTable("dbo.Receipt");
            DropTable("dbo.ReceiptItem");
            DropTable("dbo.Issue");
            DropTable("dbo.IssueItem");
            DropTable("dbo.StoreLocatorItem");
            DropTable("dbo.StoreLocatorItemLog");
        }
    }
}
