/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addadjust : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdjustItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdjustId = c.Long(),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        CurrentQuantity = c.Decimal(precision: 19, scale: 4),
                        AdjustQuantity = c.Decimal(precision: 19, scale: 4),
                        AdjustUnitPrice = c.Decimal(precision: 19, scale: 4),
                        AdjustCost = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adjust", t => t.AdjustId)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .Index(t => t.AdjustId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Adjust",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        AdjustDate = c.DateTime(precision: 0),
                        SiteId = c.Long(),
                        StoreId = c.Long(),
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
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .Index(t => t.SiteId)
                .Index(t => t.StoreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdjustItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.AdjustItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.AdjustItem", "AdjustId", "dbo.Adjust");
            DropForeignKey("dbo.Adjust", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Adjust", "SiteId", "dbo.Site");
            DropIndex("dbo.Adjust", new[] { "StoreId" });
            DropIndex("dbo.Adjust", new[] { "SiteId" });
            DropIndex("dbo.AdjustItem", new[] { "ItemId" });
            DropIndex("dbo.AdjustItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.AdjustItem", new[] { "AdjustId" });
            DropTable("dbo.Adjust");
            DropTable("dbo.AdjustItem");
        }
    }
}
