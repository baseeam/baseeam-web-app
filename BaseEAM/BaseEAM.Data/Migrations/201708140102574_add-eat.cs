namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addeat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntityAttachment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(),
                        EntityType = c.String(maxLength: 64, storeType: "nvarchar"),
                        AttachmentId = c.Long(),
                        Name = c.String(maxLength: 256, storeType: "nvarchar"),
                        IsNew = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        CreatedDateTime = c.DateTime(precision: 0),
                        ModifiedUser = c.String(maxLength: 128, storeType: "nvarchar"),
                        ModifiedDateTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attachment", t => t.AttachmentId, cascadeDelete: true)
                .Index(t => t.AttachmentId);
            
            DropColumn("dbo.Attachment", "EntityId");
            DropColumn("dbo.Attachment", "EntityType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attachment", "EntityType", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Attachment", "EntityId", c => c.Long());
            DropForeignKey("dbo.EntityAttachment", "AttachmentId", "dbo.Attachment");
            DropIndex("dbo.EntityAttachment", new[] { "AttachmentId" });
            DropTable("dbo.EntityAttachment");
        }
    }
}
