namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addtenant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenant",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
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
                "dbo.TenantLease",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        TenantId = c.Long(),
                        PropertyId = c.Long(),
                        TermStartDate = c.DateTime(precision: 0),
                        TermEndDate = c.DateTime(precision: 0),
                        TermNumber = c.Int(),
                        TermPeriod = c.Int(),
                        TermIsMonthToMonth = c.Boolean(nullable: false),
                        TermRentAmount = c.Decimal(precision: 19, scale: 4),
                        TermFrequency = c.Int(),
                        FirstPaymentDate = c.DateTime(precision: 0),
                        LateFeeEnabled = c.Boolean(nullable: false),
                        GracePeriodDays = c.Int(),
                        LateFeeOption = c.Int(),
                        FlatFee = c.Decimal(precision: 19, scale: 4),
                        BaseAmountPerDay = c.Decimal(precision: 19, scale: 4),
                        AmountPerDay = c.Decimal(precision: 19, scale: 4),
                        PercentOfRentPerDay = c.Decimal(precision: 19, scale: 4),
                        StopPerDay = c.Boolean(nullable: false),
                        MaxLateFee = c.Decimal(precision: 19, scale: 4),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
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
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Property", t => t.PropertyId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.SiteId)
                .Index(t => t.TenantId)
                .Index(t => t.PropertyId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        LocationId = c.Long(),
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
                .ForeignKey("dbo.Location", t => t.LocationId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.TenantLeaseCharge",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantLeaseId = c.Long(),
                        ChargeTypeId = c.Long(),
                        ChargeAmount = c.Decimal(precision: 19, scale: 4),
                        AmountIsOverridable = c.Boolean(nullable: false),
                        ChargeDueType = c.Int(),
                        ValidFrom = c.DateTime(precision: 0),
                        ValidTo = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.ValueItem", t => t.ChargeTypeId)
                .ForeignKey("dbo.TenantLease", t => t.TenantLeaseId)
                .Index(t => t.TenantLeaseId)
                .Index(t => t.ChargeTypeId);
            
            AddColumn("dbo.Contact", "TenantId", c => c.Long());
            CreateIndex("dbo.Contact", "TenantId");
            AddForeignKey("dbo.Contact", "TenantId", "dbo.Tenant", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contact", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.TenantLeaseCharge", "TenantLeaseId", "dbo.TenantLease");
            DropForeignKey("dbo.TenantLeaseCharge", "ChargeTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.TenantLease", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.TenantLease", "SiteId", "dbo.Site");
            DropForeignKey("dbo.TenantLease", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.Property", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Property", "LocationId", "dbo.Location");
            DropForeignKey("dbo.TenantLease", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.TenantLeaseCharge", new[] { "ChargeTypeId" });
            DropIndex("dbo.TenantLeaseCharge", new[] { "TenantLeaseId" });
            DropIndex("dbo.Property", new[] { "LocationId" });
            DropIndex("dbo.Property", new[] { "SiteId" });
            DropIndex("dbo.TenantLease", new[] { "AssignmentId" });
            DropIndex("dbo.TenantLease", new[] { "PropertyId" });
            DropIndex("dbo.TenantLease", new[] { "TenantId" });
            DropIndex("dbo.TenantLease", new[] { "SiteId" });
            DropIndex("dbo.Contact", new[] { "TenantId" });
            DropColumn("dbo.Contact", "TenantId");
            DropTable("dbo.TenantLeaseCharge");
            DropTable("dbo.Property");
            DropTable("dbo.TenantLease");
            DropTable("dbo.Tenant");
        }
    }
}
