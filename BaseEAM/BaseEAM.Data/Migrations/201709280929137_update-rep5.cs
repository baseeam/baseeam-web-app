namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updaterep5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Report", "IncludeCurrentUserInQuery", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Report", "IncludeCurrentUserInQuery");
        }
    }
}
