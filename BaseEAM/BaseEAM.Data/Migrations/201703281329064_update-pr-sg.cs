/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateprsg : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.SecurityGroup_PermissionRecord", newName: "PermissionRecord_SecurityGroup");
            //RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "SecurityGroupId", newName: "__mig_tmp__0");
            //RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "PermissionRecordId", newName: "SecurityGroupId");
            //RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "__mig_tmp__0", newName: "PermissionRecordId");
            //RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "IX_SecurityGroupId", newName: "__mig_tmp__0");
            //RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "IX_PermissionRecordId", newName: "IX_SecurityGroupId");
            //RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "__mig_tmp__0", newName: "IX_PermissionRecordId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "IX_PermissionRecordId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "IX_SecurityGroupId", newName: "IX_PermissionRecordId");
            RenameIndex(table: "dbo.PermissionRecord_SecurityGroup", name: "__mig_tmp__0", newName: "IX_SecurityGroupId");
            RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "PermissionRecordId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "SecurityGroupId", newName: "PermissionRecordId");
            RenameColumn(table: "dbo.PermissionRecord_SecurityGroup", name: "__mig_tmp__0", newName: "SecurityGroupId");
            RenameTable(name: "dbo.PermissionRecord_SecurityGroup", newName: "SecurityGroup_PermissionRecord");
        }
    }
}
