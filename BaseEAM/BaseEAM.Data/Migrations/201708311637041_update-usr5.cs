namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateusr5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "POApprovalLimit", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "POApprovalLimit");
        }
    }
}
