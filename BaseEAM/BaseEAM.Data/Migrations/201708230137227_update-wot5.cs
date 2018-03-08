namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewot5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkOrderTask", "AssignedUserId", "dbo.Technician");
            DropForeignKey("dbo.WorkOrderTask", "CompletedUserId", "dbo.Technician");
            DropIndex("dbo.WorkOrderTask", new[] { "AssignedUserId" });
            DropIndex("dbo.WorkOrderTask", new[] { "CompletedUserId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.WorkOrderTask", "CompletedUserId");
            CreateIndex("dbo.WorkOrderTask", "AssignedUserId");
            AddForeignKey("dbo.WorkOrderTask", "CompletedUserId", "dbo.Technician", "Id");
            AddForeignKey("dbo.WorkOrderTask", "AssignedUserId", "dbo.Technician", "Id");
        }
    }
}
