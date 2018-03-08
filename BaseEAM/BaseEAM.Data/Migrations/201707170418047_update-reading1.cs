namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereading1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reading", "ReadingSource", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reading", "ReadingSource");
        }
    }
}
