namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpayment3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantLease", "BiMonthlyStart", c => c.Int());
            AddColumn("dbo.TenantLease", "BiMonthlyEnd", c => c.Int());
            AddColumn("dbo.TenantLeaseCharge", "ChargeDueDate", c => c.DateTime(precision: 0));
            AddColumn("dbo.TenantLeaseCharge", "ChargeDueDay", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TenantLeaseCharge", "ChargeDueDay");
            DropColumn("dbo.TenantLeaseCharge", "ChargeDueDate");
            DropColumn("dbo.TenantLease", "BiMonthlyEnd");
            DropColumn("dbo.TenantLease", "BiMonthlyStart");
        }
    }
}
