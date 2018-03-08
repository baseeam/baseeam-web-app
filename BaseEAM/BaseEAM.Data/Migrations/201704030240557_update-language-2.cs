/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatelanguage2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Language", "LanguageCulture", c => c.String(maxLength: 20, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Language", "LanguageCulture", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
        }
    }
}
