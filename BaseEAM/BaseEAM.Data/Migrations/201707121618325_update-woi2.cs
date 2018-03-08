namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewoi2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrderItem", "UnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "PlanToolHours", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "PlanToolTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.WorkOrderItem", "ActualToolHours", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ActualToolTotal", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.WorkOrderItem", "PlanUnitPrice");
            DropColumn("dbo.WorkOrderItem", "ActualUnitPrice");
            DropColumn("dbo.WorkOrderItem", "ToolHours");
            DropColumn("dbo.WorkOrderItem", "ToolTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrderItem", "ToolTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ToolHours", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ActualUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "PlanUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.WorkOrderItem", "ActualToolTotal");
            DropColumn("dbo.WorkOrderItem", "ActualToolHours");
            DropColumn("dbo.WorkOrderItem", "PlanToolTotal");
            DropColumn("dbo.WorkOrderItem", "PlanToolHours");
            DropColumn("dbo.WorkOrderItem", "UnitPrice");
        }
    }
}
