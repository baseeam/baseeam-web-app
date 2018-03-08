/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateasset3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetLocationHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssetId = c.Long(),
                        FromLocationId = c.Long(),
                        ToLocationId = c.Long(),
                        ChangedUserId = c.Long(),
                        ChangedDateTime = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.User", t => t.ChangedUserId)
                .ForeignKey("dbo.Location", t => t.FromLocationId)
                .ForeignKey("dbo.Location", t => t.ToLocationId)
                .Index(t => t.AssetId)
                .Index(t => t.FromLocationId)
                .Index(t => t.ToLocationId)
                .Index(t => t.ChangedUserId);
            
            CreateTable(
                "dbo.AssetSparePart",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssetId = c.Long(),
                        ItemId = c.Long(),
                        Quantity = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.Item", t => t.ItemId)
                .Index(t => t.AssetId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.AssetStatusHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssetId = c.Long(),
                        FromStatus = c.String(maxLength: 64, storeType: "nvarchar"),
                        ToStatus = c.String(maxLength: 64, storeType: "nvarchar"),
                        ChangedUserId = c.Long(),
                        ChangedDateTime = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.User", t => t.ChangedUserId)
                .Index(t => t.AssetId)
                .Index(t => t.ChangedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetLocationHistory", "ToLocationId", "dbo.Location");
            DropForeignKey("dbo.AssetLocationHistory", "FromLocationId", "dbo.Location");
            DropForeignKey("dbo.AssetLocationHistory", "ChangedUserId", "dbo.User");
            DropForeignKey("dbo.AssetLocationHistory", "AssetId", "dbo.Asset");
            DropForeignKey("dbo.AssetStatusHistory", "ChangedUserId", "dbo.User");
            DropForeignKey("dbo.AssetStatusHistory", "AssetId", "dbo.Asset");
            DropForeignKey("dbo.AssetSparePart", "ItemId", "dbo.Item");
            DropForeignKey("dbo.AssetSparePart", "AssetId", "dbo.Asset");
            DropIndex("dbo.AssetStatusHistory", new[] { "ChangedUserId" });
            DropIndex("dbo.AssetStatusHistory", new[] { "AssetId" });
            DropIndex("dbo.AssetSparePart", new[] { "ItemId" });
            DropIndex("dbo.AssetSparePart", new[] { "AssetId" });
            DropIndex("dbo.AssetLocationHistory", new[] { "ChangedUserId" });
            DropIndex("dbo.AssetLocationHistory", new[] { "ToLocationId" });
            DropIndex("dbo.AssetLocationHistory", new[] { "FromLocationId" });
            DropIndex("dbo.AssetLocationHistory", new[] { "AssetId" });
            DropTable("dbo.AssetStatusHistory");
            DropTable("dbo.AssetSparePart");
            DropTable("dbo.AssetLocationHistory");
        }
    }
}
