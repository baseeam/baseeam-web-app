namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateac10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutomatedAction", "MessageTemplate", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AutomatedAction", "MessageTemplate");
        }
    }
}
