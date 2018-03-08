/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addcode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Code",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HierarchyIdPath = c.String(maxLength: 64, storeType: "nvarchar"),
                        HierarchyNamePath = c.String(maxLength: 512, storeType: "nvarchar"),
                        ParentId = c.Long(),
                        CodeType = c.String(maxLength: 64, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Code", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Code", "ParentId", "dbo.Code");
            DropIndex("dbo.Code", new[] { "ParentId" });
            DropTable("dbo.Code");
        }
    }
}
