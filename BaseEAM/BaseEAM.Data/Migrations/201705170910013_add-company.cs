/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addcompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Website = c.String(maxLength: 128, storeType: "nvarchar"),
                        CompanyTypeId = c.Long(),
                        CurrencyId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ValueItem", t => t.CompanyTypeId)
                .ForeignKey("dbo.Currency", t => t.CurrencyId)
                .Index(t => t.CompanyTypeId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Position = c.String(maxLength: 64, storeType: "nvarchar"),
                        Email = c.String(maxLength: 64, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 64, storeType: "nvarchar"),
                        Fax = c.String(maxLength: 64, storeType: "nvarchar"),
                        CompanyId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Company", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Company_Address",
                c => new
                    {
                        CompanyId = c.Long(nullable: false),
                        AddressId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.AddressId })
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.AddressId);
            
            AddColumn("dbo.Asset", "ManufacturerId", c => c.Long());
            AddColumn("dbo.Asset", "VendorId", c => c.Long());
            CreateIndex("dbo.Asset", "ManufacturerId");
            CreateIndex("dbo.Asset", "VendorId");
            AddForeignKey("dbo.Asset", "ManufacturerId", "dbo.Company", "Id");
            AddForeignKey("dbo.Asset", "VendorId", "dbo.Company", "Id");
            DropColumn("dbo.Asset", "ManufacturerName");
            DropColumn("dbo.Asset", "ManufacturerWebsite");
            DropColumn("dbo.Asset", "ManufacturerPhone");
            DropColumn("dbo.Asset", "ManufacturerEmail");
            DropColumn("dbo.Asset", "ManufacturerAddress");
            DropColumn("dbo.Asset", "VendorName");
            DropColumn("dbo.Asset", "VendorWebsite");
            DropColumn("dbo.Asset", "VendorPhone");
            DropColumn("dbo.Asset", "VendorEmail");
            DropColumn("dbo.Asset", "VendorAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Asset", "VendorAddress", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "VendorEmail", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "VendorPhone", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "VendorWebsite", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "VendorName", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "ManufacturerAddress", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "ManufacturerEmail", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "ManufacturerPhone", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "ManufacturerWebsite", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Asset", "ManufacturerName", c => c.String(maxLength: 128, storeType: "nvarchar"));
            DropForeignKey("dbo.Asset", "VendorId", "dbo.Company");
            DropForeignKey("dbo.Asset", "ManufacturerId", "dbo.Company");
            DropForeignKey("dbo.Company", "CurrencyId", "dbo.Currency");
            DropForeignKey("dbo.Contact", "CompanyId", "dbo.Company");
            DropForeignKey("dbo.Company", "CompanyTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.Company_Address", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Company_Address", "CompanyId", "dbo.Company");
            DropIndex("dbo.Company_Address", new[] { "AddressId" });
            DropIndex("dbo.Company_Address", new[] { "CompanyId" });
            DropIndex("dbo.Contact", new[] { "CompanyId" });
            DropIndex("dbo.Company", new[] { "CurrencyId" });
            DropIndex("dbo.Company", new[] { "CompanyTypeId" });
            DropIndex("dbo.Asset", new[] { "VendorId" });
            DropIndex("dbo.Asset", new[] { "ManufacturerId" });
            DropColumn("dbo.Asset", "VendorId");
            DropColumn("dbo.Asset", "ManufacturerId");
            DropTable("dbo.Company_Address");
            DropTable("dbo.Contact");
            DropTable("dbo.Company");
        }
    }
}
