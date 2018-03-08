/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTrail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        LogXml = c.String(unicode: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 2048, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeatureAction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        DisplayOrder = c.Int(nullable: false),
                        FeatureId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feature", t => t.FeatureId, cascadeDelete: true)
                .Index(t => t.FeatureId);
            
            CreateTable(
                "dbo.Feature",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        DisplayOrder = c.Int(nullable: false),
                        ModuleId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Module", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        DisplayOrder = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CurrencyCode = c.String(maxLength: 64, storeType: "nvarchar"),
                        Rate = c.Decimal(nullable: false, precision: 19, scale: 4),
                        DisplayLocale = c.String(maxLength: 64, storeType: "nvarchar"),
                        CustomFormatting = c.String(maxLength: 64, storeType: "nvarchar"),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Description = c.String(maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LanguageCulture = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        FlagImageFileName = c.String(maxLength: 50, storeType: "nvarchar"),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocaleStringResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LanguageId = c.Long(nullable: false),
                        ResourceName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ResourceValue = c.String(nullable: false, unicode: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.ActivityLog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ActivityLogTypeId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        Comment = c.String(nullable: false, unicode: false),
                        CreatedOnUtc = c.DateTime(nullable: false, precision: 0),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityLogType", t => t.ActivityLogTypeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ActivityLogTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ActivityLogType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SystemKeyword = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserGuid = c.Guid(nullable: false),
                        LoginName = c.String(maxLength: 128, storeType: "nvarchar"),
                        LoginPassword = c.String(maxLength: 128, storeType: "nvarchar"),
                        //Username = c.String(maxLength: 128, storeType: "nvarchar"),
                        Email = c.String(maxLength: 128, storeType: "nvarchar"),
                        PasswordFormatId = c.Long(nullable: false),
                        PasswordFormat = c.Int(nullable: false),
                        PasswordSalt = c.String(unicode: false),
                        Active = c.Boolean(nullable: false),
                        IsSystemAccount = c.Boolean(nullable: false),
                        DateOfBirth = c.DateTime(precision: 0),
                        AddressCountry = c.String(maxLength: 256, storeType: "nvarchar"),
                        AddressState = c.String(maxLength: 256, storeType: "nvarchar"),
                        AddressCity = c.String(maxLength: 256, storeType: "nvarchar"),
                        Address = c.String(maxLength: 256, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 128, storeType: "nvarchar"),
                        Cellphone = c.String(maxLength: 128, storeType: "nvarchar"),
                        Fax = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsPasswordChangeRequired = c.Boolean(nullable: false),
                        PasswordLastChanged = c.DateTime(precision: 0),
                        FailedLoginAttempts = c.Int(),
                        IsBanned = c.Boolean(nullable: false),
                        BannedDateFrom = c.DateTime(precision: 0),
                        IsActiveDirectoryUser = c.Boolean(nullable: false),
                        ActiveDirectoryDomain = c.String(maxLength: 128, storeType: "nvarchar"),
                        LastLoginTime = c.DateTime(precision: 0),
                        LastIpAddress = c.String(maxLength: 128, storeType: "nvarchar"),
                        WebApiEnabled = c.Boolean(nullable: false),
                        PublicKey = c.String(maxLength: 256, storeType: "nvarchar"),
                        SecretKey = c.String(maxLength: 256, storeType: "nvarchar"),
                        LastApiRequest = c.DateTime(precision: 0),
                        SupervisorId = c.Long(),
                        LanguageId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Language", t => t.LanguageId)
                .ForeignKey("dbo.User", t => t.SupervisorId)
                .Index(t => t.SupervisorId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.UserPasswordHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginPassword = c.String(maxLength: 128, storeType: "nvarchar"),
                        UserId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SecurityGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionRecord",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ModuleId = c.Long(),
                        FeatureId = c.Long(),
                        FeatureActionId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feature", t => t.FeatureId, cascadeDelete: true)
                .ForeignKey("dbo.FeatureAction", t => t.FeatureActionId, cascadeDelete: true)
                .ForeignKey("dbo.Module", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId)
                .Index(t => t.FeatureId)
                .Index(t => t.FeatureActionId);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDevice",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        Platform = c.String(maxLength: 128, storeType: "nvarchar"),
                        DeviceType = c.String(maxLength: 128, storeType: "nvarchar"),
                        DeviceToken = c.String(maxLength: 256, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LogLevelId = c.Long(nullable: false),
                        ShortMessage = c.String(nullable: false, unicode: false),
                        FullMessage = c.String(unicode: false),
                        UserId = c.Long(),
                        CreatedOnUtc = c.DateTime(nullable: false, precision: 0),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        Number = c.String(maxLength: 128, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SecurityGroup_PermissionRecord",
                c => new
                    {
                        SecurityGroupId = c.Long(nullable: false),
                        PermissionRecordId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupId, t.PermissionRecordId })
                .ForeignKey("dbo.PermissionRecord", t => t.SecurityGroupId, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroup", t => t.PermissionRecordId, cascadeDelete: true)
                .Index(t => t.SecurityGroupId)
                .Index(t => t.PermissionRecordId);
            
            CreateTable(
                "dbo.Site_SecurityGroup",
                c => new
                    {
                        SiteId = c.Long(nullable: false),
                        SecurityGroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteId, t.SecurityGroupId })
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroup", t => t.SecurityGroupId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.SecurityGroupId);
            
            CreateTable(
                "dbo.SecurityGroup_User",
                c => new
                    {
                        SecurityGroupId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupId, t.UserId })
                .ForeignKey("dbo.SecurityGroup", t => t.SecurityGroupId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SecurityGroupId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Log", "UserId", "dbo.User");
            DropForeignKey("dbo.ActivityLog", "UserId", "dbo.User");
            DropForeignKey("dbo.UserDevice", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "SupervisorId", "dbo.User");
            DropForeignKey("dbo.SecurityGroup_User", "UserId", "dbo.User");
            DropForeignKey("dbo.SecurityGroup_User", "SecurityGroupId", "dbo.SecurityGroup");
            DropForeignKey("dbo.Site_SecurityGroup", "SecurityGroupId", "dbo.SecurityGroup");
            DropForeignKey("dbo.Site_SecurityGroup", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SecurityGroup_PermissionRecord", "PermissionRecordId", "dbo.SecurityGroup");
            DropForeignKey("dbo.SecurityGroup_PermissionRecord", "SecurityGroupId", "dbo.PermissionRecord");
            DropForeignKey("dbo.PermissionRecord", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.PermissionRecord", "FeatureActionId", "dbo.FeatureAction");
            DropForeignKey("dbo.PermissionRecord", "FeatureId", "dbo.Feature");
            DropForeignKey("dbo.UserPasswordHistory", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.ActivityLog", "ActivityLogTypeId", "dbo.ActivityLogType");
            DropForeignKey("dbo.LocaleStringResource", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.FeatureAction", "FeatureId", "dbo.Feature");
            DropForeignKey("dbo.Feature", "ModuleId", "dbo.Module");
            DropIndex("dbo.SecurityGroup_User", new[] { "UserId" });
            DropIndex("dbo.SecurityGroup_User", new[] { "SecurityGroupId" });
            DropIndex("dbo.Site_SecurityGroup", new[] { "SecurityGroupId" });
            DropIndex("dbo.Site_SecurityGroup", new[] { "SiteId" });
            DropIndex("dbo.SecurityGroup_PermissionRecord", new[] { "PermissionRecordId" });
            DropIndex("dbo.SecurityGroup_PermissionRecord", new[] { "SecurityGroupId" });
            DropIndex("dbo.Log", new[] { "UserId" });
            DropIndex("dbo.UserDevice", new[] { "UserId" });
            DropIndex("dbo.PermissionRecord", new[] { "FeatureActionId" });
            DropIndex("dbo.PermissionRecord", new[] { "FeatureId" });
            DropIndex("dbo.PermissionRecord", new[] { "ModuleId" });
            DropIndex("dbo.UserPasswordHistory", new[] { "UserId" });
            DropIndex("dbo.User", new[] { "LanguageId" });
            DropIndex("dbo.User", new[] { "SupervisorId" });
            DropIndex("dbo.ActivityLog", new[] { "UserId" });
            DropIndex("dbo.ActivityLog", new[] { "ActivityLogTypeId" });
            DropIndex("dbo.LocaleStringResource", new[] { "LanguageId" });
            DropIndex("dbo.Feature", new[] { "ModuleId" });
            DropIndex("dbo.FeatureAction", new[] { "FeatureId" });
            DropTable("dbo.SecurityGroup_User");
            DropTable("dbo.Site_SecurityGroup");
            DropTable("dbo.SecurityGroup_PermissionRecord");
            DropTable("dbo.Log");
            DropTable("dbo.UserDevice");
            DropTable("dbo.Site");
            DropTable("dbo.PermissionRecord");
            DropTable("dbo.SecurityGroup");
            DropTable("dbo.UserPasswordHistory");
            DropTable("dbo.User");
            DropTable("dbo.ActivityLogType");
            DropTable("dbo.ActivityLog");
            DropTable("dbo.LocaleStringResource");
            DropTable("dbo.Language");
            DropTable("dbo.Currency");
            DropTable("dbo.Module");
            DropTable("dbo.Feature");
            DropTable("dbo.FeatureAction");
            DropTable("dbo.Setting");
            DropTable("dbo.AuditTrail");
        }
    }
}
