/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updateadjust5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Adjust", "PhysicalCountId", c => c.Long());
            AddColumn("dbo.PhysicalCount", "AdjustId", c => c.Long());
            CreateIndex("dbo.Adjust", "PhysicalCountId");
            CreateIndex("dbo.PhysicalCount", "AdjustId");
            AddForeignKey("dbo.PhysicalCount", "AdjustId", "dbo.Adjust", "Id");
            AddForeignKey("dbo.Adjust", "PhysicalCountId", "dbo.PhysicalCount", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adjust", "PhysicalCountId", "dbo.PhysicalCount");
            DropForeignKey("dbo.PhysicalCount", "AdjustId", "dbo.Adjust");
            DropIndex("dbo.PhysicalCount", new[] { "AdjustId" });
            DropIndex("dbo.Adjust", new[] { "PhysicalCountId" });
            DropColumn("dbo.PhysicalCount", "AdjustId");
            DropColumn("dbo.Adjust", "PhysicalCountId");
        }
    }
}
