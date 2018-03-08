namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpmmeter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PMMeterFrequency",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PreventiveMaintenanceId = c.Long(),
                        Frequency = c.Decimal(precision: 19, scale: 4),
                        MeterId = c.Long(),
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
                .ForeignKey("dbo.PreventiveMaintenance", t => t.PreventiveMaintenanceId)
                .Index(t => t.PreventiveMaintenanceId)
                .Index(t => t.MeterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PMMeterFrequency", "PreventiveMaintenanceId", "dbo.PreventiveMaintenance");
            DropForeignKey("dbo.PMMeterFrequency", "MeterId", "dbo.Meter");
            DropIndex("dbo.PMMeterFrequency", new[] { "MeterId" });
            DropIndex("dbo.PMMeterFrequency", new[] { "PreventiveMaintenanceId" });
            DropTable("dbo.PMMeterFrequency");
        }
    }
}
