namespace BaseEAM.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenantLease", "DueFrequency", c => c.Int());
            DropColumn("dbo.TenantLease", "TermFrequency");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TenantLease", "TermFrequency", c => c.Int());
            DropColumn("dbo.TenantLease", "DueFrequency");
        }
    }
}
