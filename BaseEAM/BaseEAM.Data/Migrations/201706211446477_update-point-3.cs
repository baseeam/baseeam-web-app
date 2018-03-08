/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepoint3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reading", "PointId", "dbo.Point");
            DropIndex("dbo.Reading", new[] { "PointId" });
            AddColumn("dbo.PointMeterLineItem", "LastReadingValue", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PointMeterLineItem", "LastDateOfReading", c => c.DateTime(precision: 0));
            AddColumn("dbo.PointMeterLineItem", "LastReadingUser", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Reading", "PointMeterLineItemId", c => c.Long());
            CreateIndex("dbo.Reading", "PointMeterLineItemId");
            AddForeignKey("dbo.Reading", "PointMeterLineItemId", "dbo.PointMeterLineItem", "Id");
            DropColumn("dbo.Point", "LastReadingValue");
            DropColumn("dbo.Point", "LastDateOfReading");
            DropColumn("dbo.Point", "LastReadingUser");
            DropColumn("dbo.Reading", "PointId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reading", "PointId", c => c.Long());
            AddColumn("dbo.Point", "LastReadingUser", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Point", "LastDateOfReading", c => c.DateTime(precision: 0));
            AddColumn("dbo.Point", "LastReadingValue", c => c.Decimal(precision: 19, scale: 4));
            DropForeignKey("dbo.Reading", "PointMeterLineItemId", "dbo.PointMeterLineItem");
            DropIndex("dbo.Reading", new[] { "PointMeterLineItemId" });
            DropColumn("dbo.Reading", "PointMeterLineItemId");
            DropColumn("dbo.PointMeterLineItem", "LastReadingUser");
            DropColumn("dbo.PointMeterLineItem", "LastDateOfReading");
            DropColumn("dbo.PointMeterLineItem", "LastReadingValue");
            CreateIndex("dbo.Reading", "PointId");
            AddForeignKey("dbo.Reading", "PointId", "dbo.Point", "Id");
        }
    }
}
