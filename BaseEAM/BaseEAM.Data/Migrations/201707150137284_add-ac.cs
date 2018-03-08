namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addac : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutomatedAction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        WhenUsed = c.Int(),
                        Expression = c.String(maxLength: 128, storeType: "nvarchar"),
                        TriggerType = c.Int(),
                        RepeatCount = c.Int(nullable: false),
                        RepeatInterval = c.Int(nullable: false),
                        CronExpression = c.String(maxLength: 64, storeType: "nvarchar"),
                        ActionType = c.String(maxLength: 64, storeType: "nvarchar"),
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
            DropTable("dbo.AutomatedAction");
        }
    }
}
