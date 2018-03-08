namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateuser15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "DefaultSiteId", c => c.Long());
            CreateIndex("dbo.User", "DefaultSiteId");
            AddForeignKey("dbo.User", "DefaultSiteId", "dbo.Site", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "DefaultSiteId", "dbo.Site");
            DropIndex("dbo.User", new[] { "DefaultSiteId" });
            DropColumn("dbo.User", "DefaultSiteId");
        }
    }
}
