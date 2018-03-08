namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsla3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SLADefinition", "EntityType", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SLADefinition", "EntityType");
        }
    }
}
