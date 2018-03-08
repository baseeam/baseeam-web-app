/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateassignmenthistory3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssignmentHistory", "TriggeredAction", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.AssignmentHistory", "ExpectedStartDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.AssignmentHistory", "DueDateTime", c => c.DateTime(precision: 0));
            AlterColumn("dbo.AssignmentHistory", "Comment", c => c.String(maxLength: 1024, storeType: "nvarchar"));
            DropColumn("dbo.AssignmentHistory", "StartDateTime");
            DropColumn("dbo.AssignmentHistory", "EndDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssignmentHistory", "EndDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.AssignmentHistory", "StartDateTime", c => c.DateTime(precision: 0));
            AlterColumn("dbo.AssignmentHistory", "Comment", c => c.String(unicode: false));
            DropColumn("dbo.AssignmentHistory", "DueDateTime");
            DropColumn("dbo.AssignmentHistory", "ExpectedStartDateTime");
            DropColumn("dbo.AssignmentHistory", "TriggeredAction");
        }
    }
}
