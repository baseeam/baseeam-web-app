namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateassh6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssignmentHistory", "WorkflowDefinitionName", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssignmentHistory", "WorkflowDefinitionName");
        }
    }
}
