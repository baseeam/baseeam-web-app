namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatemsg1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "RecipientNames", c => c.String(unicode: false));
            AddColumn("dbo.Message", "CCRecipientNames", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Message", "CCRecipientNames");
            DropColumn("dbo.Message", "RecipientNames");
        }
    }
}
