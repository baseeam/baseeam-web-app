namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateasset6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Asset", "Barcode", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Asset", "Barcode");
        }
    }
}
