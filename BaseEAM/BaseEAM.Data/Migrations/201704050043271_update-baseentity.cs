/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatebaseentity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AuditTrail", "Number");
            DropColumn("dbo.Setting", "Number");
            DropColumn("dbo.FeatureAction", "Number");
            DropColumn("dbo.Feature", "Number");
            DropColumn("dbo.Module", "Number");
            DropColumn("dbo.Currency", "Number");
            DropColumn("dbo.Language", "Number");
            DropColumn("dbo.LocaleStringResource", "Number");
            DropColumn("dbo.ActivityLog", "Number");
            DropColumn("dbo.ActivityLogType", "Number");
            DropColumn("dbo.User", "Number");
            DropColumn("dbo.UserPasswordHistory", "Number");
            DropColumn("dbo.SecurityGroup", "Number");
            DropColumn("dbo.PermissionRecord", "Number");
            DropColumn("dbo.Site", "Number");
            DropColumn("dbo.UserDevice", "Number");
            DropColumn("dbo.Log", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Log", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.UserDevice", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Site", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.PermissionRecord", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.SecurityGroup", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.UserPasswordHistory", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.User", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.ActivityLogType", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.ActivityLog", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.LocaleStringResource", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Language", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Currency", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Module", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Feature", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.FeatureAction", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.Setting", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AddColumn("dbo.AuditTrail", "Number", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
    }
}
