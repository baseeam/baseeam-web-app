namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepmmeter2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PMMeterFrequency", "EndReading", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.PMMeterFrequency", "GeneratedReading", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.PMMeterFrequency", "End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PMMeterFrequency", "End", c => c.Decimal(precision: 19, scale: 4));
            DropColumn("dbo.PMMeterFrequency", "GeneratedReading");
            DropColumn("dbo.PMMeterFrequency", "EndReading");
        }
    }
}
