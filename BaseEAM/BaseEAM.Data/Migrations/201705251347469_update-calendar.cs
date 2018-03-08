/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatecalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calendar", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calendar", "Description");
        }
    }
}
