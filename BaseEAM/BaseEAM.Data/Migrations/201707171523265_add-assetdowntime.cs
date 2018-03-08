namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addassetdowntime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetDowntime",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssetId = c.Long(),
                        StartDateTime = c.DateTime(precision: 0),
                        EndDateTime = c.DateTime(precision: 0),
                        DowntimeTypeId = c.Long(),
                        ReportedDateTime = c.DateTime(precision: 0),
                        ReportedUserId = c.Long(),
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
                .ForeignKey("dbo.ValueItem", t => t.DowntimeTypeId)
                .ForeignKey("dbo.User", t => t.ReportedUserId)
                .Index(t => t.AssetId)
                .Index(t => t.DowntimeTypeId)
                .Index(t => t.ReportedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetDowntime", "ReportedUserId", "dbo.User");
            DropForeignKey("dbo.AssetDowntime", "DowntimeTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.AssetDowntime", "AssetId", "dbo.Asset");
            DropIndex("dbo.AssetDowntime", new[] { "ReportedUserId" });
            DropIndex("dbo.AssetDowntime", new[] { "DowntimeTypeId" });
            DropIndex("dbo.AssetDowntime", new[] { "AssetId" });
            DropTable("dbo.AssetDowntime");
        }
    }
}
