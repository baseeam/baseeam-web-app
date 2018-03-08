namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetenant5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tenant", "UserId", c => c.Long());
            CreateIndex("dbo.Tenant", "UserId");
            AddForeignKey("dbo.Tenant", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenant", "UserId", "dbo.User");
            DropIndex("dbo.Tenant", new[] { "UserId" });
            DropColumn("dbo.Tenant", "UserId");
        }
    }
}
