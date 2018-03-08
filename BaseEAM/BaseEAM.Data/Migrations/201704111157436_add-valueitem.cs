/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addvalueitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ValueItemCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
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
                "dbo.ValueItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ValueItemCategoryId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ValueItemCategory", t => t.ValueItemCategoryId, cascadeDelete: true)
                .Index(t => t.ValueItemCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ValueItem", "ValueItemCategoryId", "dbo.ValueItemCategory");
            DropIndex("dbo.ValueItem", new[] { "ValueItemCategoryId" });
            DropTable("dbo.ValueItem");
            DropTable("dbo.ValueItemCategory");
        }
    }
}
