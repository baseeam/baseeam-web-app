namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addmeterevent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeterEventHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MeterEventId = c.Long(),
                        GeneratedReading = c.Decimal(precision: 19, scale: 4),
                        IsWorkOrderCreated = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MeterEvent", t => t.MeterEventId, cascadeDelete: true)
                .Index(t => t.MeterEventId);
            
            CreateTable(
                "dbo.MeterEvent",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        DisplayOrder = c.Int(nullable: false),
                        PointId = c.Long(),
                        MeterId = c.Long(),
                        UpperLimit = c.Decimal(precision: 19, scale: 4),
                        LowerLimit = c.Decimal(precision: 19, scale: 4),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meter", t => t.MeterId)
                .ForeignKey("dbo.Point", t => t.PointId)
                .Index(t => t.PointId)
                .Index(t => t.MeterId);
            
            AddColumn("dbo.Return", "WorkOrderId", c => c.Long());
            CreateIndex("dbo.Return", "WorkOrderId");
            AddForeignKey("dbo.Return", "WorkOrderId", "dbo.WorkOrder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Return", "WorkOrderId", "dbo.WorkOrder");
            DropForeignKey("dbo.MeterEventHistory", "MeterEventId", "dbo.MeterEvent");
            DropForeignKey("dbo.MeterEvent", "PointId", "dbo.Point");
            DropForeignKey("dbo.MeterEvent", "MeterId", "dbo.Meter");
            DropIndex("dbo.Return", new[] { "WorkOrderId" });
            DropIndex("dbo.MeterEvent", new[] { "MeterId" });
            DropIndex("dbo.MeterEvent", new[] { "PointId" });
            DropIndex("dbo.MeterEventHistory", new[] { "MeterEventId" });
            DropColumn("dbo.Return", "WorkOrderId");
            DropTable("dbo.MeterEvent");
            DropTable("dbo.MeterEventHistory");
        }
    }
}
