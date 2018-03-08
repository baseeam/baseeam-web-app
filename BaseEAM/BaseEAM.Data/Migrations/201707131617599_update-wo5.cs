namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewo5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrderMiscCost", "Sequence", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrderMiscCost", "Sequence");
        }
    }
}
