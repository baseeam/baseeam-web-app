/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateagu : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AssignmentGroupUser_Site", "AssignmentGroupUserId", "dbo.AssignmentGroupUser");
            //DropForeignKey("dbo.AssignmentGroupUser_Site", "SiteId", "dbo.Site");
            //DropIndex("dbo.AssignmentGroupUser_Site", new[] { "AssignmentGroupUserId" });
            //DropIndex("dbo.AssignmentGroupUser_Site", new[] { "SiteId" });
            AddColumn("dbo.AssignmentGroupUser", "SiteId", c => c.Long());
            CreateIndex("dbo.AssignmentGroupUser", "SiteId");
            AddForeignKey("dbo.AssignmentGroupUser", "SiteId", "dbo.Site", "Id");
            //DropTable("dbo.AssignmentGroupUser_Site");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AssignmentGroupUser_Site",
                c => new
                    {
                        AssignmentGroupUserId = c.Long(nullable: false),
                        SiteId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssignmentGroupUserId, t.SiteId });
            
            DropForeignKey("dbo.AssignmentGroupUser", "SiteId", "dbo.Site");
            DropIndex("dbo.AssignmentGroupUser", new[] { "SiteId" });
            DropColumn("dbo.AssignmentGroupUser", "SiteId");
            CreateIndex("dbo.AssignmentGroupUser_Site", "SiteId");
            CreateIndex("dbo.AssignmentGroupUser_Site", "AssignmentGroupUserId");
            AddForeignKey("dbo.AssignmentGroupUser_Site", "SiteId", "dbo.Site", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssignmentGroupUser_Site", "AssignmentGroupUserId", "dbo.AssignmentGroupUser", "Id", cascadeDelete: true);
        }
    }
}
