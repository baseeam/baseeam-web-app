/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatedb1 : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.PermissionRecord_SecurityGroup", newName: "SecurityGroup_PermissionRecord");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SecurityGroup_PermissionRecord", newName: "PermissionRecord_SecurityGroup");
        }
    }
}
