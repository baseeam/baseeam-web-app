/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addorg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Country = c.String(maxLength: 256, storeType: "nvarchar"),
                        StateProvince = c.String(maxLength: 256, storeType: "nvarchar"),
                        City = c.String(maxLength: 256, storeType: "nvarchar"),
                        Address1 = c.String(maxLength: 256, storeType: "nvarchar"),
                        Address2 = c.String(maxLength: 256, storeType: "nvarchar"),
                        ZipPostalCode = c.String(maxLength: 256, storeType: "nvarchar"),
                        PhoneNumber = c.String(maxLength: 256, storeType: "nvarchar"),
                        FaxNumber = c.String(maxLength: 256, storeType: "nvarchar"),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
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
                "dbo.Organization",
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
                "dbo.Site_Address",
                c => new
                    {
                        SiteId = c.Long(nullable: false),
                        AddressId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteId, t.AddressId })
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Organization_Address",
                c => new
                    {
                        OrganizationId = c.Long(nullable: false),
                        AddressId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrganizationId, t.AddressId })
                .ForeignKey("dbo.Organization", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.OrganizationId)
                .Index(t => t.AddressId);
            
            AddColumn("dbo.Site", "OrganizationId", c => c.Long());
            CreateIndex("dbo.Site", "OrganizationId");
            AddForeignKey("dbo.Site", "OrganizationId", "dbo.Organization", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Site", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Organization_Address", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Organization_Address", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.Site_Address", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Site_Address", "SiteId", "dbo.Site");
            DropIndex("dbo.Organization_Address", new[] { "AddressId" });
            DropIndex("dbo.Organization_Address", new[] { "OrganizationId" });
            DropIndex("dbo.Site_Address", new[] { "AddressId" });
            DropIndex("dbo.Site_Address", new[] { "SiteId" });
            DropIndex("dbo.Site", new[] { "OrganizationId" });
            DropColumn("dbo.Site", "OrganizationId");
            DropTable("dbo.Organization_Address");
            DropTable("dbo.Site_Address");
            DropTable("dbo.Organization");
            DropTable("dbo.Address");
        }
    }
}
