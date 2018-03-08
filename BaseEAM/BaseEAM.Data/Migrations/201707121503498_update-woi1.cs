namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewoi1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrderItem", "PlanTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ActualTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ToolHours", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ToolRate", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "ToolTotal", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrderItem", "ToolTotal");
            DropColumn("dbo.WorkOrderItem", "ToolRate");
            DropColumn("dbo.WorkOrderItem", "ToolHours");
            DropColumn("dbo.WorkOrderItem", "ActualTotal");
            DropColumn("dbo.WorkOrderItem", "PlanTotal");
        }
    }
}
