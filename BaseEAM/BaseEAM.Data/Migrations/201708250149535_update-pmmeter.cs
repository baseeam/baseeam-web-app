namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepmmeter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PMMeterFrequency", "End", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PMMeterFrequency", "End");
        }
    }
}
