namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpayment5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantPayment", "LateFeeAmount", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantPayment", "LateFeeAmount");
        }
    }
}
