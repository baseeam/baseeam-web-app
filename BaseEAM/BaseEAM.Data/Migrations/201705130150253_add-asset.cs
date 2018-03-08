/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addasset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Asset",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HierarchyIdPath = c.String(maxLength: 64, storeType: "nvarchar"),
                        HierarchyNamePath = c.String(maxLength: 512, storeType: "nvarchar"),
                        ParentId = c.Long(),
                        SiteId = c.Long(),
                        AssetTypeId = c.Long(),
                        AssetStatusId = c.Long(),
                        LocationId = c.Long(),
                        SerialNumber = c.String(maxLength: 128, storeType: "nvarchar"),
                        ManufacturerName = c.String(maxLength: 128, storeType: "nvarchar"),
                        ManufacturerWebsite = c.String(maxLength: 128, storeType: "nvarchar"),
                        ManufacturerPhone = c.String(maxLength: 128, storeType: "nvarchar"),
                        ManufacturerEmail = c.String(maxLength: 128, storeType: "nvarchar"),
                        ManufacturerAddress = c.String(maxLength: 128, storeType: "nvarchar"),
                        VendorName = c.String(maxLength: 128, storeType: "nvarchar"),
                        VendorWebsite = c.String(maxLength: 128, storeType: "nvarchar"),
                        VendorPhone = c.String(maxLength: 128, storeType: "nvarchar"),
                        VendorEmail = c.String(maxLength: 128, storeType: "nvarchar"),
                        VendorAddress = c.String(maxLength: 128, storeType: "nvarchar"),
                        InstallationDate = c.DateTime(precision: 0),
                        InstallationCost = c.Decimal(precision: 19, scale: 4),
                        PurchasePrice = c.Decimal(precision: 19, scale: 4),
                        Period = c.Int(),
                        WarrantyStartDate = c.DateTime(precision: 0),
                        WarrantyEndDate = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ValueItem", t => t.AssetStatusId)
                .ForeignKey("dbo.ValueItem", t => t.AssetTypeId)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.Asset", t => t.ParentId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.ParentId)
                .Index(t => t.SiteId)
                .Index(t => t.AssetTypeId)
                .Index(t => t.AssetStatusId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SentDateTime = c.DateTime(precision: 0),
                        IsSuccessful = c.Boolean(nullable: false),
                        NumberOfTries = c.Int(),
                        MessageType = c.Int(),
                        Messages = c.String(unicode: false),
                        Sender = c.String(maxLength: 128, storeType: "nvarchar"),
                        Recipients = c.String(unicode: false),
                        Errors = c.String(unicode: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageTemplate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        WhereUsed = c.Int(),
                        IncludesPushNotification = c.Boolean(nullable: false),
                        PushTemplate = c.String(maxLength: 512, storeType: "nvarchar"),
                        IncludesSMS = c.Boolean(nullable: false),
                        SMSTemplate = c.String(maxLength: 64, storeType: "nvarchar"),
                        IncludesEmail = c.Boolean(nullable: false),
                        EmailSubjectTemplate = c.String(maxLength: 512, storeType: "nvarchar"),
                        EmailBodyTemplate = c.String(unicode: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Item", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Asset", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Asset", "ParentId", "dbo.Asset");
            DropForeignKey("dbo.Asset", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Asset", "AssetTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.Asset", "AssetStatusId", "dbo.ValueItem");
            DropIndex("dbo.Asset", new[] { "LocationId" });
            DropIndex("dbo.Asset", new[] { "AssetStatusId" });
            DropIndex("dbo.Asset", new[] { "AssetTypeId" });
            DropIndex("dbo.Asset", new[] { "SiteId" });
            DropIndex("dbo.Asset", new[] { "ParentId" });
            DropColumn("dbo.Item", "Description");
            DropTable("dbo.MessageTemplate");
            DropTable("dbo.Message");
            DropTable("dbo.Asset");
        }
    }
}
