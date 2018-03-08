/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addreturn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReturnItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReturnId = c.Long(),
                        IssueItemId = c.Long(),
                        ReturnQuantity = c.Decimal(precision: 19, scale: 4),
                        ReturnCost = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IssueItem", t => t.IssueItemId)
                .ForeignKey("dbo.Return", t => t.ReturnId)
                .Index(t => t.ReturnId)
                .Index(t => t.IssueItemId);
            
            CreateTable(
                "dbo.Return",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        ReturnDate = c.DateTime(precision: 0),
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
            
            AddColumn("dbo.Item", "ItemLotType", c => c.Int());
            AddColumn("dbo.Item", "ItemCostingType", c => c.Int());
            DropColumn("dbo.AdjustItem", "CurrentQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdjustItem", "CurrentQuantity", c => c.Decimal(precision: 19, scale: 4));
            DropForeignKey("dbo.ReturnItem", "ReturnId", "dbo.Return");
            DropForeignKey("dbo.Return", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Return", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ReturnItem", "IssueItemId", "dbo.IssueItem");
            DropIndex("dbo.Return", new[] { "StoreId" });
            DropIndex("dbo.Return", new[] { "SiteId" });
            DropIndex("dbo.ReturnItem", new[] { "IssueItemId" });
            DropIndex("dbo.ReturnItem", new[] { "ReturnId" });
            DropColumn("dbo.Item", "ItemCostingType");
            DropColumn("dbo.Item", "ItemLotType");
            DropTable("dbo.Return");
            DropTable("dbo.ReturnItem");
        }
    }
}
