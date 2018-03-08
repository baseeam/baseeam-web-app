namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateac6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feature", "AuditTrailEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AutomatedAction", "Description", c => c.String(maxLength: 512, storeType: "nvarchar"));
            AddColumn("dbo.AutomatedAction", "Users", c => c.String(maxLength: 512, storeType: "nvarchar"));
            AddColumn("dbo.AutomatedAction", "WorkflowDefinitionId", c => c.Long());
            AddColumn("dbo.AutomatedAction", "SetExpression", c => c.String(maxLength: 512, storeType: "nvarchar"));
            CreateIndex("dbo.AutomatedAction", "WorkflowDefinitionId");
            AddForeignKey("dbo.AutomatedAction", "WorkflowDefinitionId", "dbo.WorkflowDefinition", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AutomatedAction", "WorkflowDefinitionId", "dbo.WorkflowDefinition");
            DropIndex("dbo.AutomatedAction", new[] { "WorkflowDefinitionId" });
            DropColumn("dbo.AutomatedAction", "SetExpression");
            DropColumn("dbo.AutomatedAction", "WorkflowDefinitionId");
            DropColumn("dbo.AutomatedAction", "Users");
            DropColumn("dbo.AutomatedAction", "Description");
            DropColumn("dbo.Feature", "AuditTrailEnabled");
        }
    }
}
