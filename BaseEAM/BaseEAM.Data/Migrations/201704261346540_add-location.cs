/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addlocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HierarchyIdPath = c.String(maxLength: 64, storeType: "nvarchar"),
                        HierarchyNamePath = c.String(maxLength: 512, storeType: "nvarchar"),
                        ParentId = c.Long(),
                        SiteId = c.Long(),
                        LocationTypeId = c.Long(),
                        LocationStatusId = c.Long(),
                        AddressId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.ValueItem", t => t.LocationStatusId)
                .ForeignKey("dbo.ValueItem", t => t.LocationTypeId)
                .ForeignKey("dbo.Location", t => t.ParentId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.ParentId)
                .Index(t => t.SiteId)
                .Index(t => t.LocationTypeId)
                .Index(t => t.LocationStatusId)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Location", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Location", "ParentId", "dbo.Location");
            DropForeignKey("dbo.Location", "LocationTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.Location", "LocationStatusId", "dbo.ValueItem");
            DropForeignKey("dbo.Location", "AddressId", "dbo.Address");
            DropIndex("dbo.Location", new[] { "AddressId" });
            DropIndex("dbo.Location", new[] { "LocationStatusId" });
            DropIndex("dbo.Location", new[] { "LocationTypeId" });
            DropIndex("dbo.Location", new[] { "SiteId" });
            DropIndex("dbo.Location", new[] { "ParentId" });
            DropTable("dbo.Location");
        }
    }
}
