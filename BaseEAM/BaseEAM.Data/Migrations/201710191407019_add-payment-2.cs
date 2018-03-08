namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpayment2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantPayment", "TenantLeaseId", c => c.Long());
            CreateIndex("dbo.TenantPayment", "TenantLeaseId");
            AddForeignKey("dbo.TenantPayment", "TenantLeaseId", "dbo.TenantLease", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenantPayment", "TenantLeaseId", "dbo.TenantLease");
            DropIndex("dbo.TenantPayment", new[] { "TenantLeaseId" });
            DropColumn("dbo.TenantPayment", "TenantLeaseId");
        }
    }
}
