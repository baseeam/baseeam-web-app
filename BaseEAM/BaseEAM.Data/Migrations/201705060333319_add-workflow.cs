/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addworkflow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignmentHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentType = c.String(maxLength: 64, storeType: "nvarchar"),
                        AssignmentAmount = c.Decimal(nullable: false, precision: 19, scale: 4),
                        Comment = c.String(unicode: false),
                        StartDateTime = c.DateTime(precision: 0),
                        EndDateTime = c.DateTime(precision: 0),
                        WorkflowInstanceId = c.String(maxLength: 64, storeType: "nvarchar"),
                        WorkflowVersion = c.Int(),
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
                "dbo.Assignment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentType = c.String(maxLength: 64, storeType: "nvarchar"),
                        AssignmentAmount = c.Decimal(precision: 19, scale: 4),
                        Comment = c.String(unicode: false),
                        StartDateTime = c.DateTime(precision: 0),
                        EndDateTime = c.DateTime(precision: 0),
                        WorkflowInstanceId = c.String(maxLength: 64, storeType: "nvarchar"),
                        WorkflowVersion = c.Int(),
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
                "dbo.UserGroup",
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
                "dbo.WorkflowDefinition",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
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
                "dbo.WorkflowDefinitionVersion",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkflowDefinitionId = c.Long(),
                        WorkflowVersion = c.Int(),
                        WorkflowXamlFileName = c.String(maxLength: 64, storeType: "nvarchar"),
                        WorkflowXamlFileContent = c.String(unicode: false),
                        WorkflowPictureFileName = c.String(maxLength: 64, storeType: "nvarchar"),
                        WorkflowPictureFileContent = c.Binary(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkflowDefinition", t => t.WorkflowDefinitionId, cascadeDelete: true)
                .Index(t => t.WorkflowDefinitionId);
            
            CreateTable(
                "dbo.Assignment_User",
                c => new
                    {
                        AssignmentId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssignmentId, t.UserId })
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AssignmentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroup_User",
                c => new
                    {
                        UserGroupId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroupId, t.UserId })
                .ForeignKey("dbo.UserGroup", t => t.UserGroupId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserGroupId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkflowDefinitionVersion", "WorkflowDefinitionId", "dbo.WorkflowDefinition");
            DropForeignKey("dbo.UserGroup_User", "UserId", "dbo.User");
            DropForeignKey("dbo.UserGroup_User", "UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.Assignment_User", "UserId", "dbo.User");
            DropForeignKey("dbo.Assignment_User", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.UserGroup_User", new[] { "UserId" });
            DropIndex("dbo.UserGroup_User", new[] { "UserGroupId" });
            DropIndex("dbo.Assignment_User", new[] { "UserId" });
            DropIndex("dbo.Assignment_User", new[] { "AssignmentId" });
            DropIndex("dbo.WorkflowDefinitionVersion", new[] { "WorkflowDefinitionId" });
            DropTable("dbo.UserGroup_User");
            DropTable("dbo.Assignment_User");
            DropTable("dbo.WorkflowDefinitionVersion");
            DropTable("dbo.WorkflowDefinition");
            DropTable("dbo.UserGroup");
            DropTable("dbo.Assignment");
            DropTable("dbo.AssignmentHistory");
        }
    }
}
