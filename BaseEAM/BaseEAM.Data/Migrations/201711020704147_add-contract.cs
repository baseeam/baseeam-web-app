namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcontract : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContractType = c.Int(),
                        StartDate = c.DateTime(precision: 0),
                        EndDate = c.DateTime(precision: 0),
                        Total = c.Decimal(precision: 19, scale: 4),
                        SiteId = c.Long(),
                        WorkCategoryId = c.Long(),
                        WorkTypeId = c.Long(),
                        VendorId = c.Long(),
                        SupervisorId = c.Long(),
                        Number = c.String(maxLength: 64, storeType: "nvarchar"),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Priority = c.Int(),
                        AssignmentId = c.Long(),
                        CreatedUserId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignment", t => t.AssignmentId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.User", t => t.SupervisorId)
                .ForeignKey("dbo.Company", t => t.VendorId)
                .ForeignKey("dbo.ValueItem", t => t.WorkCategoryId)
                .ForeignKey("dbo.ValueItem", t => t.WorkTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.WorkCategoryId)
                .Index(t => t.WorkTypeId)
                .Index(t => t.VendorId)
                .Index(t => t.SupervisorId)
                .Index(t => t.AssignmentId);
            
            CreateTable(
                "dbo.ContractTerm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Sequence = c.Int(),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        ContractId = c.Long(),
                        Version = c.Int(nullable: false),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contract", t => t.ContractId)
                .Index(t => t.ContractId);
            
            AddColumn("dbo.Contact", "ContractId", c => c.Long());
            AddColumn("dbo.Tenant", "AddressId", c => c.Long());
            CreateIndex("dbo.Contact", "ContractId");
            CreateIndex("dbo.Tenant", "AddressId");
            AddForeignKey("dbo.Contact", "ContractId", "dbo.Contract", "Id");
            AddForeignKey("dbo.Tenant", "AddressId", "dbo.Address", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenant", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Contact", "ContractId", "dbo.Contract");
            DropForeignKey("dbo.Contract", "WorkTypeId", "dbo.ValueItem");
            DropForeignKey("dbo.Contract", "WorkCategoryId", "dbo.ValueItem");
            DropForeignKey("dbo.Contract", "VendorId", "dbo.Company");
            DropForeignKey("dbo.Contract", "SupervisorId", "dbo.User");
            DropForeignKey("dbo.Contract", "SiteId", "dbo.Site");
            DropForeignKey("dbo.ContractTerm", "ContractId", "dbo.Contract");
            DropForeignKey("dbo.Contract", "AssignmentId", "dbo.Assignment");
            DropIndex("dbo.Tenant", new[] { "AddressId" });
            DropIndex("dbo.ContractTerm", new[] { "ContractId" });
            DropIndex("dbo.Contract", new[] { "AssignmentId" });
            DropIndex("dbo.Contract", new[] { "SupervisorId" });
            DropIndex("dbo.Contract", new[] { "VendorId" });
            DropIndex("dbo.Contract", new[] { "WorkTypeId" });
            DropIndex("dbo.Contract", new[] { "WorkCategoryId" });
            DropIndex("dbo.Contract", new[] { "SiteId" });
            DropIndex("dbo.Contact", new[] { "ContractId" });
            DropColumn("dbo.Tenant", "AddressId");
            DropColumn("dbo.Contact", "ContractId");
            DropTable("dbo.ContractTerm");
            DropTable("dbo.Contract");
        }
    }
}
