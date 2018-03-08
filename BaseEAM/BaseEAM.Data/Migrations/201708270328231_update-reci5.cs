namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereci5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiptItem", "ReceiptUnitOfMeasureId", c => c.Long());
            AddColumn("dbo.ReceiptItem", "ReceiptQuantity", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.ReceiptItem", "ReceiptUnitPrice", c => c.Decimal(precision: 19, scale: 4));
            CreateIndex("dbo.ReceiptItem", "ReceiptUnitOfMeasureId");
            AddForeignKey("dbo.ReceiptItem", "ReceiptUnitOfMeasureId", "dbo.UnitOfMeasure", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiptItem", "ReceiptUnitOfMeasureId", "dbo.UnitOfMeasure");
            DropIndex("dbo.ReceiptItem", new[] { "ReceiptUnitOfMeasureId" });
            DropColumn("dbo.ReceiptItem", "ReceiptUnitPrice");
            DropColumn("dbo.ReceiptItem", "ReceiptQuantity");
            DropColumn("dbo.ReceiptItem", "ReceiptUnitOfMeasureId");
        }
    }
}
