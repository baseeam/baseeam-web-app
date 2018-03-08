namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatestoreitem1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreItem", "PurchaseRequestId", c => c.Long());
            CreateIndex("dbo.StoreItem", "PurchaseRequestId");
            AddForeignKey("dbo.StoreItem", "PurchaseRequestId", "dbo.PurchaseRequest", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreItem", "PurchaseRequestId", "dbo.PurchaseRequest");
            DropIndex("dbo.StoreItem", new[] { "PurchaseRequestId" });
            DropColumn("dbo.StoreItem", "PurchaseRequestId");
        }
    }
}
