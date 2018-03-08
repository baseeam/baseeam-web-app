namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateass5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignment", "WorkflowDefinitionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignment", "WorkflowDefinitionId");
        }
    }
}
