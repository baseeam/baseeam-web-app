namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateassh5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssignmentHistory", "WorkflowDefinitionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssignmentHistory", "WorkflowDefinitionId");
        }
    }
}
