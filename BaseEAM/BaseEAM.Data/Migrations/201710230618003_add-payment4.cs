namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpayment4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantPayment", "TenantLeasePaymentScheduleId", c => c.Long());
            AddColumn("dbo.TenantPayment", "TenantLeaseChargeId", c => c.Long());
            CreateIndex("dbo.TenantPayment", "TenantLeasePaymentScheduleId");
            CreateIndex("dbo.TenantPayment", "TenantLeaseChargeId");
            AddForeignKey("dbo.TenantPayment", "TenantLeaseChargeId", "dbo.TenantLeaseCharge", "Id");
            AddForeignKey("dbo.TenantPayment", "TenantLeasePaymentScheduleId", "dbo.TenantLeasePaymentSchedule", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantPayment", "TenantLeasePaymentScheduleId", "dbo.TenantLeasePaymentSchedule");
            DropForeignKey("dbo.TenantPayment", "TenantLeaseChargeId", "dbo.TenantLeaseCharge");
            DropIndex("dbo.TenantPayment", new[] { "TenantLeaseChargeId" });
            DropIndex("dbo.TenantPayment", new[] { "TenantLeasePaymentScheduleId" });
            DropColumn("dbo.TenantPayment", "TenantLeaseChargeId");
            DropColumn("dbo.TenantPayment", "TenantLeasePaymentScheduleId");
        }
    }
}
