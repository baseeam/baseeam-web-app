namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesla1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SLAInstance", "Closed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SLAInstance", "Closed");
        }
    }
}
