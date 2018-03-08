namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantLeasePaymentSchedule",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantLeaseId = c.Long(),
                        DueAmount = c.Decimal(precision: 19, scale: 4),
                        DueDate = c.DateTime(precision: 0),
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
                .ForeignKey("dbo.TenantLease", t => t.TenantLeaseId)
                .Index(t => t.TenantLeaseId);
            
            CreateTable(
                "dbo.TenantPayment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SiteId = c.Long(),
                        TenantId = c.Long(),
                        PropertyId = c.Long(),
                        DueDate = c.DateTime(precision: 0),
                        ChargeTypeId = c.Long(),
                        DueAmount = c.Decimal(precision: 19, scale: 4),
                        DaysLateCount = c.Int(nullable: false),
                        CollectedAmount = c.Decimal(precision: 19, scale: 4),
                        BalanceAmount = c.Decimal(precision: 19, scale: 4),
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
                .ForeignKey("dbo.Property", t => t.PropertyId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Tenant", t => t.TenantId)
                .Index(t => t.SiteId)
                .Index(t => t.TenantId)
                .Index(t => t.PropertyId)
                .Index(t => t.ChargeTypeId);
            
            CreateTable(
                "dbo.TenantPaymentCollection",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantPaymentId = c.Long(),
                        ReceivedDate = c.DateTime(precision: 0),
                        ReceivedAmount = c.Decimal(precision: 19, scale: 4),
                        CheckNum = c.String(maxLength: 128, storeType: "nvarchar"),
                        PaymentMethod = c.Int(),
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
                .ForeignKey("dbo.TenantPayment", t => t.TenantPaymentId)
                .Index(t => t.TenantPaymentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantPaymentCollection", "TenantPaymentId", "dbo.TenantPayment");
            DropForeignKey("dbo.TenantPayment", "TenantId", "dbo.Tenant");
            DropForeignKey("dbo.TenantPayment", "SiteId", "dbo.Site");
            DropForeignKey("dbo.TenantPayment", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.TenantPayment", "ChargeTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.TenantLeasePaymentSchedule", "TenantLeaseId", "dbo.TenantLease");
            DropIndex("dbo.TenantPaymentCollection", new[] { "TenantPaymentId" });
            DropIndex("dbo.TenantPayment", new[] { "ChargeTypeId" });
            DropIndex("dbo.TenantPayment", new[] { "PropertyId" });
            DropIndex("dbo.TenantPayment", new[] { "TenantId" });
            DropIndex("dbo.TenantPayment", new[] { "SiteId" });
            DropIndex("dbo.TenantLeasePaymentSchedule", new[] { "TenantLeaseId" });
            DropTable("dbo.TenantPaymentCollection");
            DropTable("dbo.TenantPayment");
            DropTable("dbo.TenantLeasePaymentSchedule");
        }
    }
}
