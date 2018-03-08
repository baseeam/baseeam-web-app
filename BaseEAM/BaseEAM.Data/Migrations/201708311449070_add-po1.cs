namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addpo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseRequest", "RequestorId", c => c.Long());
            CreateIndex("dbo.PurchaseRequest", "RequestorId");
            AddForeignKey("dbo.PurchaseRequest", "RequestorId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseRequest", "RequestorId", "dbo.User");
            DropIndex("dbo.PurchaseRequest", new[] { "RequestorId" });
            DropColumn("dbo.PurchaseRequest", "RequestorId");
        }
    }
}
