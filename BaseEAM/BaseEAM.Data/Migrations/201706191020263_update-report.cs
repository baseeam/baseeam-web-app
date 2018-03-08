/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatereport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Report_SecurityGroup",
                c => new
                    {
                        ReportId = c.Long(nullable: false),
                        SecurityGroupId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportId, t.SecurityGroupId })
                .ForeignKey("dbo.Report", t => t.ReportId, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroup", t => t.SecurityGroupId, cascadeDelete: true)
                .Index(t => t.ReportId)
                .Index(t => t.SecurityGroupId);
            
            AddColumn("dbo.ReportColumn", "DataType", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Report_SecurityGroup", "SecurityGroupId", "dbo.SecurityGroup");
            DropForeignKey("dbo.Report_SecurityGroup", "ReportId", "dbo.Report");
            DropIndex("dbo.Report_SecurityGroup", new[] { "SecurityGroupId" });
            DropIndex("dbo.Report_SecurityGroup", new[] { "ReportId" });
            DropColumn("dbo.ReportColumn", "DataType");
            DropTable("dbo.Report_SecurityGroup");
        }
    }
}
