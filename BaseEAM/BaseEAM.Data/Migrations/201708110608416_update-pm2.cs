namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepm2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreventiveMaintenance", "PMStatusId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "TempExpectedStartDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.PreventiveMaintenance", "TempWorkDueDateTime", c => c.DateTime(precision: 0));
            CreateIndex("dbo.PreventiveMaintenance", "PMStatusId");
            AddForeignKey("dbo.PreventiveMaintenance", "PMStatusId", "dbo.ValueItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreventiveMaintenance", "PMStatusId", "dbo.ValueItem");
            DropIndex("dbo.PreventiveMaintenance", new[] { "PMStatusId" });
            DropColumn("dbo.PreventiveMaintenance", "TempWorkDueDateTime");
            DropColumn("dbo.PreventiveMaintenance", "TempExpectedStartDateTime");
            DropColumn("dbo.PreventiveMaintenance", "PMStatusId");
        }
    }
}
