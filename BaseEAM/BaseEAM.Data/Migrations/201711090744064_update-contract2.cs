namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecontract2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrder", "ContractId", c => c.Long());
            AddColumn("dbo.PreventiveMaintenance", "ContractId", c => c.Long());
            CreateIndex("dbo.PreventiveMaintenance", "ContractId");
            CreateIndex("dbo.WorkOrder", "ContractId");
            AddForeignKey("dbo.PreventiveMaintenance", "ContractId", "dbo.Contract", "Id");
            AddForeignKey("dbo.WorkOrder", "ContractId", "dbo.Contract", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrder", "ContractId", "dbo.Contract");
            DropForeignKey("dbo.PreventiveMaintenance", "ContractId", "dbo.Contract");
            DropIndex("dbo.WorkOrder", new[] { "ContractId" });
            DropIndex("dbo.PreventiveMaintenance", new[] { "ContractId" });
            DropColumn("dbo.PreventiveMaintenance", "ContractId");
            DropColumn("dbo.WorkOrder", "ContractId");
        }
    }
}
