namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewot6 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WorkOrderTask", "AssignedUserId");
            CreateIndex("dbo.WorkOrderTask", "CompletedUserId");
            AddForeignKey("dbo.WorkOrderTask", "AssignedUserId", "dbo.User", "Id");
            AddForeignKey("dbo.WorkOrderTask", "CompletedUserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderTask", "CompletedUserId", "dbo.User");
            DropForeignKey("dbo.WorkOrderTask", "AssignedUserId", "dbo.User");
            DropIndex("dbo.WorkOrderTask", new[] { "CompletedUserId" });
            DropIndex("dbo.WorkOrderTask", new[] { "AssignedUserId" });
        }
    }
}
