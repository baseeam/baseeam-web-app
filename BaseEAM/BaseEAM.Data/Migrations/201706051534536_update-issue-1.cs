/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateissue1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreLocatorReservation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StoreLocatorId = c.Long(),
                        ItemId = c.Long(),
                        QuantityReserved = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .Index(t => t.StoreLocatorId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreLocatorReservation", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.StoreLocatorReservation", "ItemId", "dbo.Item");
            DropIndex("dbo.StoreLocatorReservation", new[] { "ItemId" });
            DropIndex("dbo.StoreLocatorReservation", new[] { "StoreLocatorId" });
            DropTable("dbo.StoreLocatorReservation");
        }
    }
}
