namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsla2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SLATerm", "TrackingBaseDateTime", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.SLATerm", "TrackingDateTime", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.SLATerm", "TrackingFrom");
            DropColumn("dbo.SLATerm", "TrackingField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SLATerm", "TrackingField", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.SLATerm", "TrackingFrom", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.SLATerm", "TrackingDateTime");
            DropColumn("dbo.SLATerm", "TrackingBaseDateTime");
        }
    }
}
