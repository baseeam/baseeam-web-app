namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addimport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportProfile",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileTypeId = c.Int(),
                        EntityType = c.String(unicode: false),
                        LastRunStartDateTime = c.DateTime(precision: 0),
                        LastRunEndDateTime = c.DateTime(precision: 0),
                        ImportFileName = c.String(maxLength: 128, storeType: "nvarchar"),
                        LogFileName = c.String(maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Feature", "ImportEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feature", "ImportEnabled");
            DropTable("dbo.ImportProfile");
        }
    }
}
