/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addassignmentgroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignmentGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssignmentGroupUser",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssignmentGroupId = c.Long(),
                        UserId = c.Long(),
                        IsDefaultUser = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssignmentGroup", t => t.AssignmentGroupId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.AssignmentGroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AssignmentGroupUser_Site",
                c => new
                    {
                        AssignmentGroupUserId = c.Long(nullable: false),
                        SiteId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssignmentGroupUserId, t.SiteId })
                .ForeignKey("dbo.AssignmentGroupUser", t => t.AssignmentGroupUserId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.AssignmentGroupUserId)
                .Index(t => t.SiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignmentGroupUser", "UserId", "dbo.User");
            DropForeignKey("dbo.AssignmentGroupUser_Site", "SiteId", "dbo.Site");
            DropForeignKey("dbo.AssignmentGroupUser_Site", "AssignmentGroupUserId", "dbo.AssignmentGroupUser");
            DropForeignKey("dbo.AssignmentGroupUser", "AssignmentGroupId", "dbo.AssignmentGroup");
            DropIndex("dbo.AssignmentGroupUser_Site", new[] { "SiteId" });
            DropIndex("dbo.AssignmentGroupUser_Site", new[] { "AssignmentGroupUserId" });
            DropIndex("dbo.AssignmentGroupUser", new[] { "UserId" });
            DropIndex("dbo.AssignmentGroupUser", new[] { "AssignmentGroupId" });
            DropTable("dbo.AssignmentGroupUser_Site");
            DropTable("dbo.AssignmentGroupUser");
            DropTable("dbo.AssignmentGroup");
        }
    }
}
