namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatepo9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrder", "RequestForQuotationVendorId", c => c.Long());
            CreateIndex("dbo.PurchaseOrder", "RequestForQuotationVendorId");
            AddForeignKey("dbo.PurchaseOrder", "RequestForQuotationVendorId", "dbo.RequestForQuotationVendor", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrder", "RequestForQuotationVendorId", "dbo.RequestForQuotationVendor");
            DropIndex("dbo.PurchaseOrder", new[] { "RequestForQuotationVendorId" });
            DropColumn("dbo.PurchaseOrder", "RequestForQuotationVendorId");
        }
    }
}
