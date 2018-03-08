namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addtask : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        AssetTypes = c.String(maxLength: 512, storeType: "nvarchar"),
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
                "dbo.Task",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Sequence = c.Int(nullable: false),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        TaskGroupId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskGroup", t => t.TaskGroupId)
                .Index(t => t.TaskGroupId);
            
            CreateTable(
                "dbo.WorkOrderTask",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkOrderId = c.Long(),
                        Sequence = c.Int(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        AssignedUserId = c.Long(),
                        Completed = c.Boolean(nullable: false),
                        CompletedUserId = c.Long(),
                        CompletedDate = c.DateTime(precision: 0),
                        HoursSpent = c.Decimal(precision: 19, scale: 4),
                        Result = c.Int(),
                        CompletionNotes = c.String(maxLength: 512, storeType: "nvarchar"),
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
                .ForeignKey("dbo.Technician", t => t.CompletedUserId)
                .ForeignKey("dbo.WorkOrder", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.AssignedUserId)
                .Index(t => t.CompletedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderTask", "WorkOrderId", "dbo.WorkOrder");
            DropForeignKey("dbo.WorkOrderTask", "CompletedUserId", "dbo.Technician");
            DropForeignKey("dbo.WorkOrderTask", "AssignedUserId", "dbo.Technician");
            DropForeignKey("dbo.Task", "TaskGroupId", "dbo.TaskGroup");
            DropIndex("dbo.WorkOrderTask", new[] { "CompletedUserId" });
            DropIndex("dbo.WorkOrderTask", new[] { "AssignedUserId" });
            DropIndex("dbo.WorkOrderTask", new[] { "WorkOrderId" });
            DropIndex("dbo.Task", new[] { "TaskGroupId" });
            DropTable("dbo.WorkOrderTask");
            DropTable("dbo.Task");
            DropTable("dbo.TaskGroup");
        }
    }
}
