namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addslq : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationSequence",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLATermId = c.Long(),
                        Sequence = c.Int(nullable: false),
                        SendingTimeHours = c.Int(nullable: false),
                        SendingTimeMinutes = c.Int(nullable: false),
                        Users = c.String(maxLength: 512, storeType: "nvarchar"),
                        MessageTemplate = c.String(maxLength: 128, storeType: "nvarchar"),
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
                .ForeignKey("dbo.SLATerm", t => t.SLATermId)
                .Index(t => t.SLATermId);
            
            CreateTable(
                "dbo.SLATerm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SLADefinitionId = c.Long(),
                        TrackingFrom = c.String(maxLength: 64, storeType: "nvarchar"),
                        TrackingField = c.String(maxLength: 64, storeType: "nvarchar"),
                        LimitHours = c.Int(nullable: false),
                        LimitMinutes = c.Int(nullable: false),
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
                "dbo.SLADefinition",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationSequence", "SLATermId", "dbo.SLATerm");
            DropForeignKey("dbo.SLATerm", "SLADefinitionId", "dbo.SLADefinition");
            DropIndex("dbo.SLATerm", new[] { "SLADefinitionId" });
            DropIndex("dbo.NotificationSequence", new[] { "SLATermId" });
            DropTable("dbo.SLADefinition");
            DropTable("dbo.SLATerm");
            DropTable("dbo.NotificationSequence");
        }
    }
}
