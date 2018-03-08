namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatefilterlok : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filter", "LookupType", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Filter", "LookupTextField", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Filter", "LookupValueField", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Filter", "LookupValueField");
            DropColumn("dbo.Filter", "LookupTextField");
            DropColumn("dbo.Filter", "LookupType");
        }
    }
}
