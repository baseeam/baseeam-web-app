/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addreport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Filter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ResourceKey = c.String(maxLength: 64, storeType: "nvarchar"),
                        ControlType = c.Int(),
                        DataType = c.Int(),
                        DataSource = c.Int(),
                        CsvTextList = c.String(maxLength: 1024, storeType: "nvarchar"),
                        CsvValueList = c.String(maxLength: 1024, storeType: "nvarchar"),
                        DbTable = c.String(maxLength: 64, storeType: "nvarchar"),
                        DbTextColumn = c.String(maxLength: 64, storeType: "nvarchar"),
                        DbValueColumn = c.String(maxLength: 64, storeType: "nvarchar"),
                        SqlQuery = c.String(unicode: false),
                        SqlTextField = c.String(maxLength: 64, storeType: "nvarchar"),
                        SqlValueField = c.String(maxLength: 64, storeType: "nvarchar"),
                        MvcController = c.String(maxLength: 64, storeType: "nvarchar"),
                        MvcAction = c.String(maxLength: 64, storeType: "nvarchar"),
                        AdditionalField = c.String(maxLength: 64, storeType: "nvarchar"),
                        AdditionalValue = c.String(maxLength: 64, storeType: "nvarchar"),
                        AutoBind = c.Boolean(nullable: false),
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
                "dbo.ReportColumn",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReportId = c.Long(),
                        ColumnName = c.String(maxLength: 64, storeType: "nvarchar"),
                        ColumnHeader = c.String(maxLength: 64, storeType: "nvarchar"),
                        FormatString = c.String(maxLength: 64, storeType: "nvarchar"),
                        ResourceKey = c.String(maxLength: 64, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Report", t => t.ReportId)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.Report",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Type = c.String(maxLength: 64, storeType: "nvarchar"),
                        TemplateType = c.Int(),
                        TemplateFileName = c.String(maxLength: 64, storeType: "nvarchar"),
                        TemplateFileBytes = c.Binary(),
                        Query = c.String(unicode: false),
                        SortExpression = c.String(maxLength: 64, storeType: "nvarchar"),
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
                "dbo.ReportFilter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReportId = c.Long(),
                        FilterId = c.Long(),
                        DisplayOrder = c.Int(),
                        DbColumn = c.String(maxLength: 64, storeType: "nvarchar"),
                        IsRequired = c.Boolean(nullable: false),
                        ParentReportFilterId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Filter", t => t.FilterId)
                .ForeignKey("dbo.ReportFilter", t => t.ParentReportFilterId)
                .ForeignKey("dbo.Report", t => t.ReportId)
                .Index(t => t.ReportId)
                .Index(t => t.FilterId)
                .Index(t => t.ParentReportFilterId);
            
            CreateTable(
                "dbo.Script",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Type = c.String(maxLength: 64, storeType: "nvarchar"),
                        SourceCode = c.String(unicode: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportColumn", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportFilter", "ReportId", "dbo.Report");
            DropForeignKey("dbo.ReportFilter", "ParentReportFilterId", "dbo.ReportFilter");
            DropForeignKey("dbo.ReportFilter", "FilterId", "dbo.Filter");
            DropIndex("dbo.ReportFilter", new[] { "ParentReportFilterId" });
            DropIndex("dbo.ReportFilter", new[] { "FilterId" });
            DropIndex("dbo.ReportFilter", new[] { "ReportId" });
            DropIndex("dbo.ReportColumn", new[] { "ReportId" });
            DropTable("dbo.Script");
            DropTable("dbo.ReportFilter");
            DropTable("dbo.Report");
            DropTable("dbo.ReportColumn");
            DropTable("dbo.Filter");
        }
    }
}
