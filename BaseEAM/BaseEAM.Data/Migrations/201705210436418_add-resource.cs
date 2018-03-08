/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addresource : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calendar",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsSunday = c.Boolean(nullable: false),
                        IsMonday = c.Boolean(nullable: false),
                        IsTuesday = c.Boolean(nullable: false),
                        IsWednesday = c.Boolean(nullable: false),
                        IsThursday = c.Boolean(nullable: false),
                        IsFriday = c.Boolean(nullable: false),
                        IsSaturday = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CalendarNonWorking",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CalendarId = c.Long(),
                        NonWorkingDate = c.DateTime(precision: 0),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendar", t => t.CalendarId, cascadeDelete: true)
                .Index(t => t.CalendarId);
            
            CreateTable(
                "dbo.Shift",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        CalendarId = c.Long(),
                        StartDay = c.Int(),
                        DaysInPattern = c.Int(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendar", t => t.CalendarId)
                .Index(t => t.CalendarId);
            
            CreateTable(
                "dbo.ShiftPattern",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Sequence = c.Int(),
                        StartTime = c.DateTime(precision: 0),
                        EndTime = c.DateTime(precision: 0),
                        ShiftId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shift", t => t.ShiftId)
                .Index(t => t.ShiftId);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        SiteId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Technician",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        CalendarId = c.Long(),
                        ShiftId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendar", t => t.CalendarId)
                .ForeignKey("dbo.Shift", t => t.ShiftId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CalendarId)
                .Index(t => t.ShiftId);
            
            CreateTable(
                "dbo.Team_Technician",
                c => new
                    {
                        TeamId = c.Long(nullable: false),
                        TechnicianId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamId, t.TechnicianId })
                .ForeignKey("dbo.Team", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Technician", t => t.TechnicianId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.TechnicianId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team_Technician", "TechnicianId", "dbo.Technician");
            DropForeignKey("dbo.Team_Technician", "TeamId", "dbo.Team");
            DropForeignKey("dbo.Technician", "UserId", "dbo.User");
            DropForeignKey("dbo.Technician", "ShiftId", "dbo.Shift");
            DropForeignKey("dbo.Technician", "CalendarId", "dbo.Calendar");
            DropForeignKey("dbo.Team", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ShiftPattern", "ShiftId", "dbo.Shift");
            DropForeignKey("dbo.Shift", "CalendarId", "dbo.Calendar");
            DropForeignKey("dbo.CalendarNonWorking", "CalendarId", "dbo.Calendar");
            DropIndex("dbo.Team_Technician", new[] { "TechnicianId" });
            DropIndex("dbo.Team_Technician", new[] { "TeamId" });
            DropIndex("dbo.Technician", new[] { "ShiftId" });
            DropIndex("dbo.Technician", new[] { "CalendarId" });
            DropIndex("dbo.Technician", new[] { "UserId" });
            DropIndex("dbo.Team", new[] { "SiteId" });
            DropIndex("dbo.ShiftPattern", new[] { "ShiftId" });
            DropIndex("dbo.Shift", new[] { "CalendarId" });
            DropIndex("dbo.CalendarNonWorking", new[] { "CalendarId" });
            DropTable("dbo.Team_Technician");
            DropTable("dbo.Technician");
            DropTable("dbo.Team");
            DropTable("dbo.ShiftPattern");
            DropTable("dbo.Shift");
            DropTable("dbo.CalendarNonWorking");
            DropTable("dbo.Calendar");
        }
    }
}
