/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereceipt2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipt", "AssignmentId", c => c.Long());
            CreateIndex("dbo.Receipt", "AssignmentId");
            AddForeignKey("dbo.Receipt", "AssignmentId", "dbo.Assignment", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipt", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.Receipt", new[] { "AssignmentId" });
            DropColumn("dbo.Receipt", "AssignmentId");
        }
    }
}
