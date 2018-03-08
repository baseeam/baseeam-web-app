/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addphysicalcount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhysicalCountItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PhysicalCountId = c.Long(),
                        StoreLocatorItemId = c.Long(),
                        Count = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PhysicalCount", t => t.PhysicalCountId)
                .ForeignKey("dbo.StoreLocatorItem", t => t.StoreLocatorItemId)
                .Index(t => t.PhysicalCountId)
                .Index(t => t.StoreLocatorItemId);
            
            CreateTable(
                "dbo.PhysicalCount",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        PhysicalCountDate = c.DateTime(precision: 0),
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
            DropForeignKey("dbo.PhysicalCountItem", "StoreLocatorItemId", "dbo.StoreLocatorItem");
            DropForeignKey("dbo.PhysicalCountItem", "PhysicalCountId", "dbo.PhysicalCount");
            DropForeignKey("dbo.PhysicalCount", "StoreId", "dbo.Store");
            DropForeignKey("dbo.PhysicalCount", "SiteId", "dbo.Site");
            DropIndex("dbo.PhysicalCount", new[] { "StoreId" });
            DropIndex("dbo.PhysicalCount", new[] { "SiteId" });
            DropIndex("dbo.PhysicalCountItem", new[] { "StoreLocatorItemId" });
            DropIndex("dbo.PhysicalCountItem", new[] { "PhysicalCountId" });
            DropTable("dbo.PhysicalCount");
            DropTable("dbo.PhysicalCountItem");
        }
    }
}
