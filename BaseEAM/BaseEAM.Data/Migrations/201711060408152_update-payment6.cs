namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepayment6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantPaymentCollection", "PaymentMethodId", c => c.Long());
            CreateIndex("dbo.TenantPaymentCollection", "PaymentMethodId");
            AddForeignKey("dbo.TenantPaymentCollection", "PaymentMethodId", "dbo.ValueItem", "Id");
            DropColumn("dbo.TenantPaymentCollection", "PaymentMethod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TenantPaymentCollection", "PaymentMethod", c => c.Int());
            DropForeignKey("dbo.TenantPaymentCollection", "PaymentMethodId", "dbo.ValueItem");
            DropIndex("dbo.TenantPaymentCollection", new[] { "PaymentMethodId" });
            DropColumn("dbo.TenantPaymentCollection", "PaymentMethodId");
        }
    }
}
