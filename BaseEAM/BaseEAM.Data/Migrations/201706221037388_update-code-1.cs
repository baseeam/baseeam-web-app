/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatecode1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Code", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Code", "Description");
        }
    }
}
