namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepm9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PM_MeterEvent",
                c => new
                    {
                        PreventiveMaintenanceId = c.Long(nullable: false),
                        MeterEventId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PreventiveMaintenanceId, t.MeterEventId })
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId, cascadeDelete: true)
                .ForeignKey("dbo.MeterEvent", t => t.MeterEventId, cascadeDelete: true)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.MeterEventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PM_MeterEvent", "MeterEventId", "dbo.MeterEvent");
            DropForeignKey("dbo.PM_MeterEvent", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropIndex("dbo.PM_MeterEvent", new[] { "MeterEventId" });
            DropIndex("dbo.PM_MeterEvent", new[] { "PreventiveMaintenanceId" });
            DropTable("dbo.PM_MeterEvent");
        }
    }
}
