namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adduserdashboard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDashboard",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        DashboardLayoutType = c.Int(),
                        RegionCount = c.Int(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserDashboardVisual",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserDashboardId = c.Long(),
                        CellId = c.Int(),
                        VisualId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDashboard", t => t.UserDashboardId)
                .ForeignKey("dbo.Visual", t => t.VisualId)
                .Index(t => t.UserDashboardId)
                .Index(t => t.VisualId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDashboardVisual", "VisualId", "dbo.Visual");
            DropForeignKey("dbo.UserDashboardVisual", "UserDashboardId", "dbo.UserDashboard");
            DropForeignKey("dbo.UserDashboard", "UserId", "dbo.User");
            DropIndex("dbo.UserDashboardVisual", new[] { "VisualId" });
            DropIndex("dbo.UserDashboardVisual", new[] { "UserDashboardId" });
            DropIndex("dbo.UserDashboard", new[] { "UserId" });
            DropTable("dbo.UserDashboardVisual");
            DropTable("dbo.UserDashboard");
        }
    }
}
