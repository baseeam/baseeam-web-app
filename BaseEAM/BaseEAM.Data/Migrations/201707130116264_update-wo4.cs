namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class updatewo4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrder", "ActualStartDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.WorkOrder", "ActualEndDateTime", c => c.DateTime(precision: 0));
            AddColumn("dbo.WorkOrder", "ActualFailureGroupId", c => c.Long());
            AddColumn("dbo.WorkOrder", "ActualProblemId", c => c.Long());
            AddColumn("dbo.WorkOrder", "ActualCauseId", c => c.Long());
            AddColumn("dbo.WorkOrder", "ResolutionId", c => c.Long());
            AddColumn("dbo.WorkOrder", "ResolutionNotes", c => c.String(maxLength: 512, storeType: "nvarchar"));
            CreateIndex("dbo.WorkOrder", "ActualFailureGroupId");
            CreateIndex("dbo.WorkOrder", "ActualProblemId");
            CreateIndex("dbo.WorkOrder", "ActualCauseId");
            CreateIndex("dbo.WorkOrder", "ResolutionId");
            AddForeignKey("dbo.WorkOrder", "ActualCauseId", "dbo.Code", "Id");
            AddForeignKey("dbo.WorkOrder", "ActualFailureGroupId", "dbo.Code", "Id");
            AddForeignKey("dbo.WorkOrder", "ActualProblemId", "dbo.Code", "Id");
            AddForeignKey("dbo.WorkOrder", "ResolutionId", "dbo.Code", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrder", "ResolutionId", "dbo.Code");
            DropForeignKey("dbo.WorkOrder", "ActualProblemId", "dbo.Code");
            DropForeignKey("dbo.WorkOrder", "ActualFailureGroupId", "dbo.Code");
            DropForeignKey("dbo.WorkOrder", "ActualCauseId", "dbo.Code");
            DropIndex("dbo.WorkOrder", new[] { "ResolutionId" });
            DropIndex("dbo.WorkOrder", new[] { "ActualCauseId" });
            DropIndex("dbo.WorkOrder", new[] { "ActualProblemId" });
            DropIndex("dbo.WorkOrder", new[] { "ActualFailureGroupId" });
            DropColumn("dbo.WorkOrder", "ResolutionNotes");
            DropColumn("dbo.WorkOrder", "ResolutionId");
            DropColumn("dbo.WorkOrder", "ActualCauseId");
            DropColumn("dbo.WorkOrder", "ActualProblemId");
            DropColumn("dbo.WorkOrder", "ActualFailureGroupId");
            DropColumn("dbo.WorkOrder", "ActualEndDateTime");
            DropColumn("dbo.WorkOrder", "ActualStartDateTime");
        }
    }
}
