namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewfd9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkflowDefinition", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkflowDefinition", "IsDefault");
        }
    }
}
