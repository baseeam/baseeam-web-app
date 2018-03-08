/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addstore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        ItemGroupId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemGroup", t => t.ItemGroupId)
                .Index(t => t.ItemGroupId);
            
            CreateTable(
                "dbo.StoreLocator",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StoreId = c.Long(),
                        IsDefault = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        SiteId = c.Long(),
                        LocationId = c.Long(),
                        StoreTypeId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.ValueItem", t => t.StoreTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.LocationId)
                .Index(t => t.StoreTypeId);
            
            AddColumn("dbo.Item", "UnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Item", "ManufacturerId", c => c.Long());
            CreateIndex("dbo.Item", "ManufacturerId");
            AddForeignKey("dbo.Item", "ManufacturerId", "dbo.Company", "Id");
            DropColumn("dbo.Item", "Manufacturer");
            DropColumn("dbo.Item", "Model");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "Model", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Item", "Manufacturer", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropForeignKey("dbo.StoreLocator", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Store", "StoreTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.Store", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Store", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ServiceItem", "ItemGroupId", "dbo.ItemGroup");
            DropForeignKey("dbo.Item", "ManufacturerId", "dbo.Company");
            DropIndex("dbo.Store", new[] { "StoreTypeId" });
            DropIndex("dbo.Store", new[] { "LocationId" });
            DropIndex("dbo.Store", new[] { "SiteId" });
            DropIndex("dbo.StoreLocator", new[] { "StoreId" });
            DropIndex("dbo.ServiceItem", new[] { "ItemGroupId" });
            DropIndex("dbo.Item", new[] { "ManufacturerId" });
            DropColumn("dbo.Item", "ManufacturerId");
            DropColumn("dbo.Item", "UnitPrice");
            DropTable("dbo.Store");
            DropTable("dbo.StoreLocator");
            DropTable("dbo.ServiceItem");
        }
    }
}
