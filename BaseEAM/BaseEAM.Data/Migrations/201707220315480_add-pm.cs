namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PMItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        StoreId = c.Long(),
                        ItemId = c.Long(),
                        StoreLocatorId = c.Long(),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        PlanQuantity = c.Decimal(precision: 19, scale: 4),
                        PlanTotal = c.Decimal(precision: 19, scale: 4),
                        PlanToolHours = c.Decimal(precision: 19, scale: 4),
                        ToolRate = c.Decimal(precision: 19, scale: 4),
                        PlanToolTotal = c.Decimal(precision: 18, scale: 2),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .ForeignKey("dbo.StoreLocator", t => t.StoreLocatorId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.StoreId)
                .Index(t => t.ItemId)
                .Index(t => t.StoreLocatorId);
            
            CreateTable(
                "dbo.PMLabor",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        TeamId = c.Long(),
                        TechnicianId = c.Long(),
                        CraftId = c.Long(),
                        PlanHours = c.Decimal(precision: 19, scale: 4),
                        StandardRate = c.Decimal(precision: 19, scale: 4),
                        OTRate = c.Decimal(precision: 19, scale: 4),
                        PlanTotal = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .ForeignKey("dbo.Team", t => t.TeamId)
                .ForeignKey("dbo.Technician", t => t.TechnicianId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.TeamId)
                .Index(t => t.TechnicianId)
                .Index(t => t.CraftId);
            
            CreateTable(
                "dbo.PMMiscCost",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        Sequence = c.Int(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        PlanUnitPrice = c.Decimal(precision: 19, scale: 4),
                        PlanQuantity = c.Decimal(precision: 19, scale: 4),
                        PlanTotal = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .Index(t => t.PreventiveMaintenanceId);
            
            CreateTable(
                "dbo.PMServiceItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        ServiceItemId = c.Long(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        UnitPrice = c.Decimal(precision: 19, scale: 4),
                        Quantity = c.Decimal(precision: 19, scale: 4),
                        Total = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .ForeignKey("dbo.ServiceItem", t => t.ServiceItemId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.ServiceItemId);
            
            CreateTable(
                "dbo.PMTask",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        Sequence = c.Int(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        AssignedUserId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Technician", t => t.AssignedUserId)
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.AssignedUserId);
            
            AddColumn("dbo.WorkOrder", "TaskGroupId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "Number", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.PreventiveMaintenance", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
            AddColumn("dbo.PreventiveMaintenance", "Priority", c => c.Int());
            AddColumn("dbo.PreventiveMaintenance", "SiteId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "AssetId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "LocationId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "WorkCategoryId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "WorkTypeId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "FailureGroupId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "SupervisorId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "TaskGroupId", c => c.Long());
            CreateIndex("dbo.WorkOrder", "TaskGroupId");
            CreateIndex("dbo.PreventiveMaintenance", "SiteId");
            CreateIndex("dbo.PreventiveMaintenance", "AssetId");
            CreateIndex("dbo.PreventiveMaintenance", "LocationId");
            CreateIndex("dbo.PreventiveMaintenance", "WorkCategoryId");
            CreateIndex("dbo.PreventiveMaintenance", "WorkTypeId");
            CreateIndex("dbo.PreventiveMaintenance", "FailureGroupId");
            CreateIndex("dbo.PreventiveMaintenance", "SupervisorId");
            CreateIndex("dbo.PreventiveMaintenance", "TaskGroupId");
            AddForeignKey("dbo.PreventiveMaintenance", "AssetId", "dbo.Asset", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "FailureGroupId", "dbo.Code", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "LocationId", "dbo.Location", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "SiteId", "dbo.Site", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "SupervisorId", "dbo.User", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "TaskGroupId", "dbo.TaskGroup", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "WorkCategoryId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.PreventiveMaintenance", "WorkTypeId", "dbo.ValueItem", "Id");
            AddForeignKey("dbo.WorkOrder", "TaskGroupId", "dbo.TaskGroup", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrder", "TaskGroupId", "dbo.TaskGroup");
            DropForeignKey("dbo.PreventiveMaintenance", "WorkTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.PreventiveMaintenance", "WorkCategoryId", "dbo.ValueItem");
            DropForeignKey("dbo.PreventiveMaintenance", "TaskGroupId", "dbo.TaskGroup");
            DropForeignKey("dbo.PreventiveMaintenance", "SupervisorId", "dbo.User");
            DropForeignKey("dbo.PreventiveMaintenance", "SiteId", "dbo.Site");
            DropForeignKey("dbo.PMTask", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMTask", "AssignedUserId", "dbo.Technician");
            DropForeignKey("dbo.PMServiceItem", "ServiceItemId", "dbo.ServiceItem");
            DropForeignKey("dbo.PMServiceItem", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMMiscCost", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMLabor", "TechnicianId", "dbo.Technician");
            DropForeignKey("dbo.PMLabor", "TeamId", "dbo.Team");
            DropForeignKey("dbo.PMLabor", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMLabor", "CraftId", "dbo.Craft");
            DropForeignKey("dbo.PMItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.PMItem", "StoreId", "dbo.Store");
            DropForeignKey("dbo.PMItem", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMItem", "ItemId", "dbo.Item");
            DropForeignKey("dbo.PreventiveMaintenance", "LocationId", "dbo.Location");
            DropForeignKey("dbo.PreventiveMaintenance", "FailureGroupId", "dbo.Code");
            DropForeignKey("dbo.PreventiveMaintenance", "AssetId", "dbo.Asset");
            DropIndex("dbo.PMTask", new[] { "AssignedUserId" });
            DropIndex("dbo.PMTask", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.PMServiceItem", new[] { "ServiceItemId" });
            DropIndex("dbo.PMServiceItem", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.PMMiscCost", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.PMLabor", new[] { "CraftId" });
            DropIndex("dbo.PMLabor", new[] { "TechnicianId" });
            DropIndex("dbo.PMLabor", new[] { "TeamId" });
            DropIndex("dbo.PMLabor", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.PMItem", new[] { "StoreLocatorId" });
            DropIndex("dbo.PMItem", new[] { "ItemId" });
            DropIndex("dbo.PMItem", new[] { "StoreId" });
            DropIndex("dbo.PMItem", new[] { "PreventiveMaintenanceId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "TaskGroupId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "SupervisorId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "FailureGroupId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "WorkTypeId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "WorkCategoryId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "LocationId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "AssetId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "SiteId" });
            DropIndex("dbo.WorkOrder", new[] { "TaskGroupId" });
            DropColumn("dbo.PreventiveMaintenance", "TaskGroupId");
            DropColumn("dbo.PreventiveMaintenance", "SupervisorId");
            DropColumn("dbo.PreventiveMaintenance", "FailureGroupId");
            DropColumn("dbo.PreventiveMaintenance", "WorkTypeId");
            DropColumn("dbo.PreventiveMaintenance", "WorkCategoryId");
            DropColumn("dbo.PreventiveMaintenance", "LocationId");
            DropColumn("dbo.PreventiveMaintenance", "AssetId");
            DropColumn("dbo.PreventiveMaintenance", "SiteId");
            DropColumn("dbo.PreventiveMaintenance", "Priority");
            DropColumn("dbo.PreventiveMaintenance", "Description");
            DropColumn("dbo.PreventiveMaintenance", "Number");
            DropColumn("dbo.WorkOrder", "TaskGroupId");
            DropTable("dbo.PMTask");
            DropTable("dbo.PMServiceItem");
            DropTable("dbo.PMMiscCost");
            DropTable("dbo.PMLabor");
            DropTable("dbo.PMItem");
        }
    }
}
