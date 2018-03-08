/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatemt3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "Subject", c => c.String(unicode: false));
            AddColumn("dbo.Message", "CCRecipients", c => c.String(unicode: false));
            AddColumn("dbo.Message", "AttachmentIds", c => c.String(maxLength: 256, storeType: "nvarchar"));
            AddColumn("dbo.MessageTemplate", "EmailSender", c => c.String(maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MessageTemplate", "EmailSender");
            DropColumn("dbo.Message", "AttachmentIds");
            DropColumn("dbo.Message", "CCRecipients");
            DropColumn("dbo.Message", "Subject");
        }
    }
}
