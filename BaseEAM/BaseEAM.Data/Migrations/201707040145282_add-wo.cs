namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addwo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PreventiveMaintenance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                "dbo.ServiceRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.WorkOrderLabor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkOrderId = c.Long(),
                        TeamId = c.Long(),
                        TechnicianId = c.Long(),
                        CraftId = c.Long(),
                        PlanHours = c.Decimal(precision: 19, scale: 4),
                        PlanRate = c.Decimal(precision: 19, scale: 4),
                        PlanTotal = c.Decimal(precision: 19, scale: 4),
                        ActualNormalHours = c.Decimal(precision: 19, scale: 4),
                        ActualStandardRate = c.Decimal(precision: 19, scale: 4),
                        ActualOTHours = c.Decimal(precision: 19, scale: 4),
                        ActualOTRate = c.Decimal(precision: 19, scale: 4),
                        ActualTotal = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Craft", t => t.CraftId)
                .ForeignKey("dbo.Team", t => t.TeamId)
                .ForeignKey("dbo.Technician", t => t.TechnicianId)
                .ForeignKey("dbo.WorkOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.TeamId)
                .Index(t => t.TechnicianId)
                .Index(t => t.CraftId);
            
            CreateTable(
                "dbo.WorkOrder",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HierarchyIdPath = c.String(maxLength: 64, storeType: "nvarchar"),
                        HierarchyNamePath = c.String(maxLength: 512, storeType: "nvarchar"),
                        ParentId = c.Long(),
                        SiteId = c.Long(),
                        AssetId = c.Long(),
                        LocationId = c.Long(),
                        WorkCategoryId = c.Long(),
                        WorkTypeId = c.Long(),
                        FailureGroupId = c.Long(),
                        ServiceRequestId = c.Long(),
                        PreventiveMaintenanceId = c.Long(),
                        RequestorName = c.String(maxLength: 64, storeType: "nvarchar"),
                        RequestorEmail = c.String(maxLength: 64, storeType: "nvarchar"),
                        RequestorPhone = c.String(maxLength: 64, storeType: "nvarchar"),
                        RequestedDateTime = c.DateTime(precision: 0),
                        SupervisorId = c.Long(),
                        ExpectedStartDateTime = c.DateTime(precision: 0),
                        DueDateTime = c.DateTime(precision: 0),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.AssetId)
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Code", t => t.FailureGroupId)
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.WorkOrder", t => t.ParentId)
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .ForeignKey("dbo.ServiceRequest", t => t.ServiceRequestId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.User", t => t.SupervisorId)
                .ForeignKey("dbo.ValueItem", t => t.WorkCategoryId)
                .ForeignKey("dbo.ValueItem", t => t.WorkTypeId)
                .Index(t => t.ParentId)
                .Index(t => t.SiteId)
                .Index(t => t.AssetId)
                .Index(t => t.LocationId)
                .Index(t => t.WorkCategoryId)
                .Index(t => t.WorkTypeId)
                .Index(t => t.FailureGroupId)
                .Index(t => t.ServiceRequestId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.SupervisorId)
                .Index(t => t.AssignmentId);
            
            AlterColumn("dbo.User", "Number", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AlterColumn("dbo.User", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderLabor", "WorkOrderId", "dbo.WorkOrder");
            DropForeignKey("dbo.WorkOrder", "WorkTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.WorkOrder", "WorkCategoryId", "dbo.ValueItem");
            DropForeignKey("dbo.WorkOrder", "SupervisorId", "dbo.User");
            DropForeignKey("dbo.WorkOrder", "SiteId", "dbo.Site");
            DropForeignKey("dbo.WorkOrder", "ServiceRequestId", "dbo.ServiceRequest");
            DropForeignKey("dbo.WorkOrder", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.WorkOrder", "ParentId", "dbo.WorkOrder");
            DropForeignKey("dbo.WorkOrder", "LocationId", "dbo.Location");
            DropForeignKey("dbo.WorkOrder", "FailureGroupId", "dbo.Code");
            DropForeignKey("dbo.WorkOrder", "AssignmentId", "dbo.Assignment");
            DropForeignKey("dbo.WorkOrder", "AssetId", "dbo.Asset");
            DropForeignKey("dbo.WorkOrderLabor", "TechnicianId", "dbo.Technician");
            DropForeignKey("dbo.WorkOrderLabor", "TeamId", "dbo.Team");
            DropForeignKey("dbo.WorkOrderLabor", "CraftId", "dbo.Craft");
            DropForeignKey("dbo.ServiceRequest", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.WorkOrder", new[] { "AssignmentId" });
            DropIndex("dbo.WorkOrder", new[] { "SupervisorId" });
            DropIndex("dbo.WorkOrder", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.WorkOrder", new[] { "ServiceRequestId" });
            DropIndex("dbo.WorkOrder", new[] { "FailureGroupId" });
            DropIndex("dbo.WorkOrder", new[] { "WorkTypeId" });
            DropIndex("dbo.WorkOrder", new[] { "WorkCategoryId" });
            DropIndex("dbo.WorkOrder", new[] { "LocationId" });
            DropIndex("dbo.WorkOrder", new[] { "AssetId" });
            DropIndex("dbo.WorkOrder", new[] { "SiteId" });
            DropIndex("dbo.WorkOrder", new[] { "ParentId" });
            DropIndex("dbo.WorkOrderLabor", new[] { "CraftId" });
            DropIndex("dbo.WorkOrderLabor", new[] { "TechnicianId" });
            DropIndex("dbo.WorkOrderLabor", new[] { "TeamId" });
            DropIndex("dbo.WorkOrderLabor", new[] { "WorkOrderId" });
            DropIndex("dbo.ServiceRequest", new[] { "AssignmentId" });
            AlterColumn("dbo.User", "Description", c => c.String(unicode: false));
            AlterColumn("dbo.User", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            DropTable("dbo.WorkOrder");
            DropTable("dbo.WorkOrderLabor");
            DropTable("dbo.ServiceRequest");
            DropTable("dbo.PreventiveMaintenance");
        }
    }
}
