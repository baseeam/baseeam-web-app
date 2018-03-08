/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addreading : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Point",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LocationId = c.Long(),
                        AssetId = c.Long(),
                        MeterGroupId = c.Long(),
                        LastReadingValue = c.Decimal(precision: 19, scale: 4),
                        LastDateOfReading = c.DateTime(precision: 0),
                        LastReadingUser = c.String(maxLength: 64, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.AssetId)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.MeterGroup", t => t.MeterGroupId)
                .Index(t => t.LocationId)
                .Index(t => t.AssetId)
                .Index(t => t.MeterGroupId);
            
            CreateTable(
                "dbo.PointMeterLineItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        PointId = c.Long(),
                        MeterId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meter", t => t.MeterId)
                .ForeignKey("dbo.Point", t => t.PointId, cascadeDelete: true)
                .Index(t => t.PointId)
                .Index(t => t.MeterId);
            
            CreateTable(
                "dbo.Reading",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PointId = c.Long(),
                        ReadingValue = c.Decimal(precision: 19, scale: 4),
                        DateOfReading = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Point", t => t.PointId)
                .Index(t => t.PointId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reading", "PointId", "dbo.Point");
            DropForeignKey("dbo.PointMeterLineItem", "PointId", "dbo.Point");
            DropForeignKey("dbo.PointMeterLineItem", "MeterId", "dbo.Meter");
            DropForeignKey("dbo.Point", "MeterGroupId", "dbo.MeterGroup");
            DropForeignKey("dbo.Point", "LocationId", "dbo.Location");
            DropForeignKey("dbo.Point", "AssetId", "dbo.Asset");
            DropIndex("dbo.Reading", new[] { "PointId" });
            DropIndex("dbo.PointMeterLineItem", new[] { "MeterId" });
            DropIndex("dbo.PointMeterLineItem", new[] { "PointId" });
            DropIndex("dbo.Point", new[] { "MeterGroupId" });
            DropIndex("dbo.Point", new[] { "AssetId" });
            DropIndex("dbo.Point", new[] { "LocationId" });
            DropTable("dbo.Reading");
            DropTable("dbo.PointMeterLineItem");
            DropTable("dbo.Point");
        }
    }
}
