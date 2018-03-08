/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateissue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Issue", "IssueTo", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Issue", "IssueTo");
        }
    }
}
