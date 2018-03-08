namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewopmitem : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PMItem", "PlanToolTotal");
            DropColumn("dbo.WorkOrderItem", "PlanToolTotal");
            DropColumn("dbo.WorkOrderItem", "ActualToolTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrderItem", "ActualToolTotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderItem", "PlanToolTotal", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PMItem", "PlanToolTotal", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
