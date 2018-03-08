/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateusertimezone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "TimeZoneId", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "TimeZoneId");
        }
    }
}
