namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserType");
        }
    }
}
