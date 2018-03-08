namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepo7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipt", "PurchaseOrderId", c => c.Long());
            AddColumn("dbo.PurchaseOrderItem", "ReceiveToStoreId", c => c.Long());
            AddColumn("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId", c => c.Long());
            AddColumn("dbo.PurchaseOrder", "IsSent", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestForQuotation", "IsSent", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Receipt", "PurchaseOrderId");
            CreateIndex("dbo.PurchaseOrderItem", "ReceiveToStoreId");
            CreateIndex("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId");
            AddForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreId", "dbo.Store", "Id");
            AddForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId", "dbo.StoreLocator", "Id");
            AddForeignKey("dbo.Receipt", "PurchaseOrderId", "dbo.PurchaseOrder", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipt", "PurchaseOrderId", "dbo.PurchaseOrder");
            DropForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.PurchaseOrderItem", "ReceiveToStoreId", "dbo.Store");
            DropIndex("dbo.PurchaseOrderItem", new[] { "ReceiveToStoreLocatorId" });
            DropIndex("dbo.PurchaseOrderItem", new[] { "ReceiveToStoreId" });
            DropIndex("dbo.Receipt", new[] { "PurchaseOrderId" });
            DropColumn("dbo.RequestForQuotation", "IsSent");
            DropColumn("dbo.PurchaseOrder", "IsSent");
            DropColumn("dbo.PurchaseOrderItem", "ReceiveToStoreLocatorId");
            DropColumn("dbo.PurchaseOrderItem", "ReceiveToStoreId");
            DropColumn("dbo.Receipt", "PurchaseOrderId");
        }
    }
}
