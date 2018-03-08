namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatephc6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhysicalCountItem", "CurrentQuantity", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhysicalCountItem", "CurrentQuantity");
        }
    }
}
