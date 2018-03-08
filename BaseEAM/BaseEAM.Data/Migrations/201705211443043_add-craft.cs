/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addcraft : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Craft",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        StandardRate = c.Decimal(precision: 19, scale: 4),
                        OvertimeRate = c.Decimal(precision: 19, scale: 4),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Technician", "CraftId", c => c.Long());
            CreateIndex("dbo.Technician", "CraftId");
            AddForeignKey("dbo.Technician", "CraftId", "dbo.Craft", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Technician", "CraftId", "dbo.Craft");
            DropIndex("dbo.Technician", new[] { "CraftId" });
            DropColumn("dbo.Technician", "CraftId");
            DropTable("dbo.Craft");
        }
    }
}
