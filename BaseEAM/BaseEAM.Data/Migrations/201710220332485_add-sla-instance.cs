namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addslainstance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SLAInstance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        SLADefinitionId = c.Long(),
                        Violated = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SLADefinition", t => t.SLADefinitionId)
                .Index(t => t.SLADefinitionId);
            
            CreateTable(
                "dbo.SLAInstanceTerm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLAInstanceId = c.Long(),
                        SLATermId = c.Long(),
                        TrackingBaseDateTime = c.DateTime(precision: 0),
                        TrackingDateTime = c.DateTime(precision: 0),
                        Violated = c.Boolean(nullable: false),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SLAInstance", t => t.SLAInstanceId)
                .ForeignKey("dbo.SLATerm", t => t.SLATermId)
                .Index(t => t.SLAInstanceId)
                .Index(t => t.SLATermId);
            
            AddColumn("dbo.WorkOrder", "OnSiteDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.WorkOrder", "SLAEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceRequest", "SLAEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.SLATerm", "TrackingBaseField", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.SLATerm", "TrackingField", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropColumn("dbo.SLATerm", "TrackingBaseDateTime");
            DropColumn("dbo.SLATerm", "TrackingDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SLATerm", "TrackingDateTime", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.SLATerm", "TrackingBaseDateTime", c => c.String(maxLength: 64, storeType: "nvarchar"));
            DropForeignKey("dbo.SLAInstanceTerm", "SLATermId", "dbo.SLATerm");
            DropForeignKey("dbo.SLAInstanceTerm", "SLAInstanceId", "dbo.SLAInstance");
            DropForeignKey("dbo.SLAInstance", "SLADefinitionId", "dbo.SLADefinition");
            DropIndex("dbo.SLAInstanceTerm", new[] { "SLATermId" });
            DropIndex("dbo.SLAInstanceTerm", new[] { "SLAInstanceId" });
            DropIndex("dbo.SLAInstance", new[] { "SLADefinitionId" });
            DropColumn("dbo.SLATerm", "TrackingField");
            DropColumn("dbo.SLATerm", "TrackingBaseField");
            DropColumn("dbo.ServiceRequest", "SLAEnabled");
            DropColumn("dbo.WorkOrder", "SLAEnabled");
            DropColumn("dbo.WorkOrder", "OnSiteDateTime");
            DropTable("dbo.SLAInstanceTerm");
            DropTable("dbo.SLAInstance");
        }
    }
}
