namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewbe3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "CreatedUserId", c => c.Long());
            AddColumn("dbo.WorkOrder", "CreatedUserId", c => c.Long());
            AddColumn("dbo.ServiceRequest", "CreatedUserId", c => c.Long());
            AddColumn("dbo.Receipt", "CreatedUserId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receipt", "CreatedUserId");
            DropColumn("dbo.ServiceRequest", "CreatedUserId");
            DropColumn("dbo.WorkOrder", "CreatedUserId");
            DropColumn("dbo.User", "CreatedUserId");
        }
    }
}
