namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class removeusergroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserGroup_User", "UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.UserGroup_User", "UserId", "dbo.User");
            DropIndex("dbo.UserGroup_User", new[] { "UserGroupId" });
            DropIndex("dbo.UserGroup_User", new[] { "UserId" });
            DropTable("dbo.UserGroup");
            DropTable("dbo.UserGroup_User");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserGroup_User",
                c => new
                    {
                        UserGroupId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroupId, t.UserId });
            
            CreateTable(
                "dbo.UserGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(maxLength: 512, storeType: "nvarchar"),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserGroup_User", "UserId");
            CreateIndex("dbo.UserGroup_User", "UserGroupId");
            AddForeignKey("dbo.UserGroup_User", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserGroup_User", "UserGroupId", "dbo.UserGroup", "Id", cascadeDelete: true);
        }
    }
}
