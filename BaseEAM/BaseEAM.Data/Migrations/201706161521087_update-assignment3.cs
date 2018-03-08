/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateassignment3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignment", "ExpectedStartDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.Assignment", "DueDateTime", c => c.DateTime(precision: 0));
            DropColumn("dbo.Assignment", "StartDateTime");
            DropColumn("dbo.Assignment", "EndDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignment", "EndDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.Assignment", "StartDateTime", c => c.DateTime(precision: 0));
            DropColumn("dbo.Assignment", "DueDateTime");
            DropColumn("dbo.Assignment", "ExpectedStartDateTime");
        }
    }
}
