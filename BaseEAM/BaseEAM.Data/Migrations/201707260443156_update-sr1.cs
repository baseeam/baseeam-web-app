namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatesr1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PreventiveMaintenance", "FirstWorkExpectedStartDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.PreventiveMaintenance", "FirstWorkDueDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.PreventiveMaintenance", "EndDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.PreventiveMaintenance", "FrequencyCount", c => c.Int());
            AddColumn("dbo.PreventiveMaintenance", "FrequencyType", c => c.Int());
            AddColumn("dbo.PreventiveMaintenance", "Sunday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Monday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Tuesday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Wednesday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Thursday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Friday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Saturday", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day1", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day2", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day3", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day4", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day5", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day6", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day7", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day8", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day9", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day10", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day11", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day12", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day13", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day14", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day15", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day16", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day17", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day18", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day19", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day20", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day21", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day22", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day23", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day24", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day25", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day26", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day27", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day28", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day29", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day30", c => c.Boolean(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Day31", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceRequest", "SiteId", c => c.Long());
            AddColumn("dbo.ServiceRequest", "AssetId", c => c.Long());
            AddColumn("dbo.ServiceRequest", "LocationId", c => c.Long());
            AddColumn("dbo.ServiceRequest", "RequestorType", c => c.Int());
            AddColumn("dbo.ServiceRequest", "RequestorName", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.ServiceRequest", "RequestorEmail", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.ServiceRequest", "RequestorPhone", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.ServiceRequest", "RequestedDateTime", c => c.DateTime(precision: 0));
            CreateIndex("dbo.ServiceRequest", "SiteId");
            CreateIndex("dbo.ServiceRequest", "AssetId");
            CreateIndex("dbo.ServiceRequest", "LocationId");
            AddForeignKey("dbo.ServiceRequest", "AssetId", "dbo.Asset", "Id");
            AddForeignKey("dbo.ServiceRequest", "LocationId", "dbo.Location", "Id");
            AddForeignKey("dbo.ServiceRequest", "SiteId", "dbo.Site", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequest", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ServiceRequest", "LocationId", "dbo.Location");
            DropForeignKey("dbo.ServiceRequest", "AssetId", "dbo.Asset");
            DropIndex("dbo.ServiceRequest", new[] { "LocationId" });
            DropIndex("dbo.ServiceRequest", new[] { "AssetId" });
            DropIndex("dbo.ServiceRequest", new[] { "SiteId" });
            DropColumn("dbo.ServiceRequest", "RequestedDateTime");
            DropColumn("dbo.ServiceRequest", "RequestorPhone");
            DropColumn("dbo.ServiceRequest", "RequestorEmail");
            DropColumn("dbo.ServiceRequest", "RequestorName");
            DropColumn("dbo.ServiceRequest", "RequestorType");
            DropColumn("dbo.ServiceRequest", "LocationId");
            DropColumn("dbo.ServiceRequest", "AssetId");
            DropColumn("dbo.ServiceRequest", "SiteId");
            DropColumn("dbo.PreventiveMaintenance", "Day31");
            DropColumn("dbo.PreventiveMaintenance", "Day30");
            DropColumn("dbo.PreventiveMaintenance", "Day29");
            DropColumn("dbo.PreventiveMaintenance", "Day28");
            DropColumn("dbo.PreventiveMaintenance", "Day27");
            DropColumn("dbo.PreventiveMaintenance", "Day26");
            DropColumn("dbo.PreventiveMaintenance", "Day25");
            DropColumn("dbo.PreventiveMaintenance", "Day24");
            DropColumn("dbo.PreventiveMaintenance", "Day23");
            DropColumn("dbo.PreventiveMaintenance", "Day22");
            DropColumn("dbo.PreventiveMaintenance", "Day21");
            DropColumn("dbo.PreventiveMaintenance", "Day20");
            DropColumn("dbo.PreventiveMaintenance", "Day19");
            DropColumn("dbo.PreventiveMaintenance", "Day18");
            DropColumn("dbo.PreventiveMaintenance", "Day17");
            DropColumn("dbo.PreventiveMaintenance", "Day16");
            DropColumn("dbo.PreventiveMaintenance", "Day15");
            DropColumn("dbo.PreventiveMaintenance", "Day14");
            DropColumn("dbo.PreventiveMaintenance", "Day13");
            DropColumn("dbo.PreventiveMaintenance", "Day12");
            DropColumn("dbo.PreventiveMaintenance", "Day11");
            DropColumn("dbo.PreventiveMaintenance", "Day10");
            DropColumn("dbo.PreventiveMaintenance", "Day9");
            DropColumn("dbo.PreventiveMaintenance", "Day8");
            DropColumn("dbo.PreventiveMaintenance", "Day7");
            DropColumn("dbo.PreventiveMaintenance", "Day6");
            DropColumn("dbo.PreventiveMaintenance", "Day5");
            DropColumn("dbo.PreventiveMaintenance", "Day4");
            DropColumn("dbo.PreventiveMaintenance", "Day3");
            DropColumn("dbo.PreventiveMaintenance", "Day2");
            DropColumn("dbo.PreventiveMaintenance", "Day1");
            DropColumn("dbo.PreventiveMaintenance", "Saturday");
            DropColumn("dbo.PreventiveMaintenance", "Friday");
            DropColumn("dbo.PreventiveMaintenance", "Thursday");
            DropColumn("dbo.PreventiveMaintenance", "Wednesday");
            DropColumn("dbo.PreventiveMaintenance", "Tuesday");
            DropColumn("dbo.PreventiveMaintenance", "Monday");
            DropColumn("dbo.PreventiveMaintenance", "Sunday");
            DropColumn("dbo.PreventiveMaintenance", "FrequencyType");
            DropColumn("dbo.PreventiveMaintenance", "FrequencyCount");
            DropColumn("dbo.PreventiveMaintenance", "EndDateTime");
            DropColumn("dbo.PreventiveMaintenance", "FirstWorkDueDateTime");
            DropColumn("dbo.PreventiveMaintenance", "FirstWorkExpectedStartDateTime");
        }
    }
}
