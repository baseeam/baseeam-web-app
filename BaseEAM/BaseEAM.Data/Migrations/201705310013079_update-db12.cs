/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatedb12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreLocatorItemLog", "BatchDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreLocatorItemLog", "BatchDate");
        }
    }
}
