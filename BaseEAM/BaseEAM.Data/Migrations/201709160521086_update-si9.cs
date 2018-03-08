namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatesi9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PMServiceItem", "PlanUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMServiceItem", "PlanQuantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMServiceItem", "PlanTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "PlanUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "PlanQuantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "PlanTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "ActualUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "ActualQuantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "ActualTotal", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.PMServiceItem", "UnitPrice");
            DropColumn("dbo.PMServiceItem", "Quantity");
            DropColumn("dbo.PMServiceItem", "Total");
            DropColumn("dbo.WorkOrderServiceItem", "UnitPrice");
            DropColumn("dbo.WorkOrderServiceItem", "Quantity");
            DropColumn("dbo.WorkOrderServiceItem", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrderServiceItem", "Total", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "Quantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderServiceItem", "UnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMServiceItem", "Total", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMServiceItem", "Quantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMServiceItem", "UnitPrice", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.WorkOrderServiceItem", "ActualTotal");
            DropColumn("dbo.WorkOrderServiceItem", "ActualQuantity");
            DropColumn("dbo.WorkOrderServiceItem", "ActualUnitPrice");
            DropColumn("dbo.WorkOrderServiceItem", "PlanTotal");
            DropColumn("dbo.WorkOrderServiceItem", "PlanQuantity");
            DropColumn("dbo.WorkOrderServiceItem", "PlanUnitPrice");
            DropColumn("dbo.PMServiceItem", "PlanTotal");
            DropColumn("dbo.PMServiceItem", "PlanQuantity");
            DropColumn("dbo.PMServiceItem", "PlanUnitPrice");
        }
    }
}
