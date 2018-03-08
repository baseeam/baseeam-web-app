namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addwi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkflowInstance",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InstanceId = c.String(maxLength: 64, storeType: "nvarchar"),
                        InstanceData = c.String(unicode: false),
                        Version = c.Int(nullable: false),
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
            DropTable("dbo.WorkflowInstance");
        }
    }
}
