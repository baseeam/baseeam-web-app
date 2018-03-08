namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateah3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssignmentHistory", "AssignedUsers", c => c.String(maxLength: 256, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssignmentHistory", "AssignedUsers");
        }
    }
}
