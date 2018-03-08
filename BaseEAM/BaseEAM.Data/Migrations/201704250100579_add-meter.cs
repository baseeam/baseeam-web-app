/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addmeter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeterGroup",
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
                "dbo.MeterLineItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        MeterGroupId = c.Long(),
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
                .ForeignKey("dbo.MeterGroup", t => t.MeterGroupId, cascadeDelete: true)
                .Index(t => t.MeterGroupId)
                .Index(t => t.MeterId);
            
            CreateTable(
                "dbo.Meter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        MeterTypeId = c.Long(),
                        UnitOfMeasureId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ValueItem", t => t.MeterTypeId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.UnitOfMeasureId)
                .Index(t => t.MeterTypeId)
                .Index(t => t.UnitOfMeasureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeterLineItem", "MeterGroupId", "dbo.MeterGroup");
            DropForeignKey("dbo.MeterLineItem", "MeterId", "dbo.Meter");
            DropForeignKey("dbo.Meter", "UnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.Meter", "MeterTypeId", "dbo.ValueItem");
            DropIndex("dbo.Meter", new[] { "UnitOfMeasureId" });
            DropIndex("dbo.Meter", new[] { "MeterTypeId" });
            DropIndex("dbo.MeterLineItem", new[] { "MeterId" });
            DropIndex("dbo.MeterLineItem", new[] { "MeterGroupId" });
            DropTable("dbo.Meter");
            DropTable("dbo.MeterLineItem");
            DropTable("dbo.MeterGroup");
        }
    }
}
