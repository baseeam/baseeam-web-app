namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatephc5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PhysicalCountItem", "StoreLocatorItemId", "dbo.StoreLocatorItem");
            DropIndex("dbo.PhysicalCountItem", new[] { "StoreLocatorItemId" });
            AddColumn("dbo.PhysicalCountItem", "StoreLocatorId", c => c.Long());
            AddColumn("dbo.PhysicalCountItem", "ItemId", c => c.Long());
            CreateIndex("dbo.PhysicalCountItem", "StoreLocatorId");
            CreateIndex("dbo.PhysicalCountItem", "ItemId");
            AddForeignKey("dbo.PhysicalCountItem", "ItemId", "dbo.Item", "Id");
            AddForeignKey("dbo.PhysicalCountItem", "StoreLocatorId", "dbo.StoreLocator", "Id");
            DropColumn("dbo.PhysicalCountItem", "StoreLocatorItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhysicalCountItem", "StoreLocatorItemId", c => c.Long());
            DropForeignKey("dbo.PhysicalCountItem", "StoreLocatorId", "dbo.StoreLocator");
            DropForeignKey("dbo.PhysicalCountItem", "ItemId", "dbo.Item");
            DropIndex("dbo.PhysicalCountItem", new[] { "ItemId" });
            DropIndex("dbo.PhysicalCountItem", new[] { "StoreLocatorId" });
            DropColumn("dbo.PhysicalCountItem", "ItemId");
            DropColumn("dbo.PhysicalCountItem", "StoreLocatorId");
            CreateIndex("dbo.PhysicalCountItem", "StoreLocatorItemId");
            AddForeignKey("dbo.PhysicalCountItem", "StoreLocatorItemId", "dbo.StoreLocatorItem", "Id");
        }
    }
}
