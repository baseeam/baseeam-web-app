namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addautonumber : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutoNumber",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        FormatString = c.String(unicode: false),
                        LastNumber = c.Int(nullable: false),
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
            DropTable("dbo.AutoNumber");
        }
    }
}
