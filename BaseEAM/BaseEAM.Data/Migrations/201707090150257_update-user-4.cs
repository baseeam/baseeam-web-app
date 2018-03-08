namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateuser4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Username", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
    }
}
