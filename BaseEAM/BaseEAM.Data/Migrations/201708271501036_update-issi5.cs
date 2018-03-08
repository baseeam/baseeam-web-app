namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateissi5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IssueItem", "Quantity", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IssueItem", "Quantity");
        }
    }
}
