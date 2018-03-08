/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adduom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnitConversion",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromUnitOfMeasureId = c.Long(),
                        ToUnitOfMeasureId = c.Long(),
                        ConversionFactor = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnitOfMeasure", t => t.FromUnitOfMeasureId)
                .ForeignKey("dbo.UnitOfMeasure", t => t.ToUnitOfMeasureId)
                .Index(t => t.FromUnitOfMeasureId)
                .Index(t => t.ToUnitOfMeasureId);
            
            CreateTable(
                "dbo.UnitOfMeasure",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Abbreviation = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnitConversion", "ToUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropForeignKey("dbo.UnitConversion", "FromUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropIndex("dbo.UnitConversion", new[] { "ToUnitOfMeasureId" });
            DropIndex("dbo.UnitConversion", new[] { "FromUnitOfMeasureId" });
            DropTable("dbo.UnitOfMeasure");
            DropTable("dbo.UnitConversion");
        }
    }
}
