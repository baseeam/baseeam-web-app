namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateac1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutomatedAction", "ActionTypeId", c => c.Long());
            CreateIndex("dbo.AutomatedAction", "ActionTypeId");
            AddForeignKey("dbo.AutomatedAction", "ActionTypeId", "dbo.ValueItem", "Id");
            DropColumn("dbo.AutomatedAction", "ActionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AutomatedAction", "ActionType", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropForeignKey("dbo.AutomatedAction", "ActionTypeId", "dbo.ValueItem");
            DropIndex("dbo.AutomatedAction", new[] { "ActionTypeId" });
            DropColumn("dbo.AutomatedAction", "ActionTypeId");
        }
    }
}
