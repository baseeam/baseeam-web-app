namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepo3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderItem", "Subtotal", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PurchaseOrderItem", "SubtotalWithTax", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrderItem", "SubtotalWithTax");
            DropColumn("dbo.PurchaseOrderItem", "Subtotal");
        }
    }
}
