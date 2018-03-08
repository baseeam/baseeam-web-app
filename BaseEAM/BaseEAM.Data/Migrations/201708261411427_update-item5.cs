namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateitem5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "Barcode", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.Item", "StockCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "StockCode", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.Item", "Barcode");
        }
    }
}
