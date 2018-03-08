/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateitem1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "ItemCategory", c => c.Int());
            AddColumn("dbo.Item", "ItemStockType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Item", "ItemStockType");
            DropColumn("dbo.Item", "ItemCategory");
        }
    }
}
