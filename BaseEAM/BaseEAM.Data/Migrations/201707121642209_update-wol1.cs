namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewol1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrderLabor", "StandardRate", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderLabor", "OTRate", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.WorkOrderLabor", "PlanRate");
            DropColumn("dbo.WorkOrderLabor", "ActualStandardRate");
            DropColumn("dbo.WorkOrderLabor", "ActualOTRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkOrderLabor", "ActualOTRate", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderLabor", "ActualStandardRate", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.WorkOrderLabor", "PlanRate", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.WorkOrderLabor", "OTRate");
            DropColumn("dbo.WorkOrderLabor", "StandardRate");
        }
    }
}
