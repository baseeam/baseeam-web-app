/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class additem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
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
                "dbo.Item",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Manufacturer = c.String(maxLength: 64, storeType: "nvarchar"),
                        Model = c.String(maxLength: 64, storeType: "nvarchar"),
                        StockCode = c.String(maxLength: 64, storeType: "nvarchar"),
                        ItemGroupId = c.Long(),
                        UnitOfMeasureId = c.Long(),
                        ItemStatusId = c.Long(),
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
                .ForeignKey("dbo.ValueItem", t => t.ItemStatusId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.UnitOfMeasureId)
                .Index(t => t.ItemGroupId)
                .Index(t => t.UnitOfMeasureId)
                .Index(t => t.ItemStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "UnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.Item", "ItemStatusId", "dbo.ValueItem");
            DropForeignKey("dbo.Item", "ItemGroupId", "dbo.ItemGroup");
            DropIndex("dbo.Item", new[] { "ItemStatusId" });
            DropIndex("dbo.Item", new[] { "UnitOfMeasureId" });
            DropIndex("dbo.Item", new[] { "ItemGroupId" });
            DropTable("dbo.Item");
            DropTable("dbo.ItemGroup");
        }
    }
}
