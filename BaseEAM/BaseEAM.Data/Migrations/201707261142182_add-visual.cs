namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addvisual : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visual",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        VisualType = c.Int(),
                        Query = c.String(unicode: false),
                        SortExpression = c.String(maxLength: 64, storeType: "nvarchar"),
                        XAxis = c.String(maxLength: 64, storeType: "nvarchar"),
                        YAxis = c.String(maxLength: 64, storeType: "nvarchar"),
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
                "dbo.VisualFilter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VisualId = c.Long(),
                        FilterId = c.Long(),
                        DisplayOrder = c.Int(),
                        DbColumn = c.String(maxLength: 64, storeType: "nvarchar"),
                        IsRequired = c.Boolean(nullable: false),
                        ResourceKey = c.String(maxLength: 64, storeType: "nvarchar"),
                        ParentReportFilterId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Filter", t => t.FilterId)
                .ForeignKey("dbo.ReportFilter", t => t.ParentReportFilterId)
                .ForeignKey("dbo.Visual", t => t.VisualId)
                .Index(t => t.VisualId)
                .Index(t => t.FilterId)
                .Index(t => t.ParentReportFilterId);
            
            CreateTable(
                "dbo.Visual_SecurityGroup",
                c => new
                    {
                        VisualId = c.Long(nullable: false),
                        SecurityGroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.VisualId, t.SecurityGroupId })
                .ForeignKey("dbo.Visual", t => t.VisualId, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroup", t => t.SecurityGroupId, cascadeDelete: true)
                .Index(t => t.VisualId)
                .Index(t => t.SecurityGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisualFilter", "VisualId", "dbo.Visual");
            DropForeignKey("dbo.VisualFilter", "ParentReportFilterId", "dbo.ReportFilter");
            DropForeignKey("dbo.VisualFilter", "FilterId", "dbo.Filter");
            DropForeignKey("dbo.Visual_SecurityGroup", "SecurityGroupId", "dbo.SecurityGroup");
            DropForeignKey("dbo.Visual_SecurityGroup", "VisualId", "dbo.Visual");
            DropIndex("dbo.Visual_SecurityGroup", new[] { "SecurityGroupId" });
            DropIndex("dbo.Visual_SecurityGroup", new[] { "VisualId" });
            DropIndex("dbo.VisualFilter", new[] { "ParentReportFilterId" });
            DropIndex("dbo.VisualFilter", new[] { "FilterId" });
            DropIndex("dbo.VisualFilter", new[] { "VisualId" });
            DropTable("dbo.Visual_SecurityGroup");
            DropTable("dbo.VisualFilter");
            DropTable("dbo.Visual");
        }
    }
}
