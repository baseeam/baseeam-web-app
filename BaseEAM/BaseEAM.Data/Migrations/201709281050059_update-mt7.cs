namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatemt7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MessageTemplate", "SMSTemplate", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MessageTemplate", "SMSTemplate", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
    }
}
