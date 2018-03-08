namespace BaseEAM.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addversion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssetDowntime", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Asset", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssetLocationHistory", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.User", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Assignment", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Language", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.LocaleStringResource", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UserPasswordHistory", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.SecurityGroup", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PermissionRecord", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Feature", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.FeatureAction", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Module", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Report", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ReportColumn", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ReportFilter", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Filter", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Site", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Organization", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Visual", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.VisualFilter", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UserDevice", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Location", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ValueItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ValueItemCategory", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssetSparePart", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Item", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ItemGroup", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Company", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Contact", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Currency", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UnitOfMeasure", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssetStatusHistory", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.MeterGroup", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.MeterLineItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Meter", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Point", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PointMeterLineItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Reading", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrder", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Code", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PreventiveMaintenance", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PMItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Store", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.StoreItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.StoreLocator", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.StoreLocatorItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.StoreLocatorItemLog", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PMLabor", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Craft", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Team", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Technician", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Calendar", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.CalendarNonWorking", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Shift", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ShiftPattern", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PMMiscCost", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PMServiceItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PMTask", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.TaskGroup", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Task", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceRequest", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrderItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrderLabor", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrderMiscCost", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrderServiceItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkOrderTask", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Attribute", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.EntityAttribute", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AuditEntityConfiguration", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AuditTrail", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AutomatedAction", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AutoNumber", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Attachment", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.EntityAttachment", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Comment", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Picture", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Message", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.MessageTemplate", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Setting", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UserDashboard", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UserDashboardVisual", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ImportProfile", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.StoreLocatorReservation", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AdjustItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Adjust", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PhysicalCount", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.PhysicalCountItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.IssueItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Issue", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ReceiptItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Receipt", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ReturnItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Return", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.TransferItem", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Transfer", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.UnitConversion", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ActivityLog", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.ActivityLogType", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Log", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.Script", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssignmentGroup", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssignmentGroupUser", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.AssignmentHistory", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkflowDefinition", "Version", c => c.Int(nullable: false));
            AddColumn("dbo.WorkflowDefinitionVersion", "Version", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkflowDefinitionVersion", "Version");
            DropColumn("dbo.WorkflowDefinition", "Version");
            DropColumn("dbo.AssignmentHistory", "Version");
            DropColumn("dbo.AssignmentGroupUser", "Version");
            DropColumn("dbo.AssignmentGroup", "Version");
            DropColumn("dbo.Script", "Version");
            DropColumn("dbo.Log", "Version");
            DropColumn("dbo.ActivityLogType", "Version");
            DropColumn("dbo.ActivityLog", "Version");
            DropColumn("dbo.UnitConversion", "Version");
            DropColumn("dbo.Transfer", "Version");
            DropColumn("dbo.TransferItem", "Version");
            DropColumn("dbo.Return", "Version");
            DropColumn("dbo.ReturnItem", "Version");
            DropColumn("dbo.Receipt", "Version");
            DropColumn("dbo.ReceiptItem", "Version");
            DropColumn("dbo.Issue", "Version");
            DropColumn("dbo.IssueItem", "Version");
            DropColumn("dbo.PhysicalCountItem", "Version");
            DropColumn("dbo.PhysicalCount", "Version");
            DropColumn("dbo.Adjust", "Version");
            DropColumn("dbo.AdjustItem", "Version");
            DropColumn("dbo.StoreLocatorReservation", "Version");
            DropColumn("dbo.ImportProfile", "Version");
            DropColumn("dbo.UserDashboardVisual", "Version");
            DropColumn("dbo.UserDashboard", "Version");
            DropColumn("dbo.Setting", "Version");
            DropColumn("dbo.MessageTemplate", "Version");
            DropColumn("dbo.Message", "Version");
            DropColumn("dbo.Picture", "Version");
            DropColumn("dbo.Comment", "Version");
            DropColumn("dbo.EntityAttachment", "Version");
            DropColumn("dbo.Attachment", "Version");
            DropColumn("dbo.AutoNumber", "Version");
            DropColumn("dbo.AutomatedAction", "Version");
            DropColumn("dbo.AuditTrail", "Version");
            DropColumn("dbo.AuditEntityConfiguration", "Version");
            DropColumn("dbo.EntityAttribute", "Version");
            DropColumn("dbo.Attribute", "Version");
            DropColumn("dbo.WorkOrderTask", "Version");
            DropColumn("dbo.WorkOrderServiceItem", "Version");
            DropColumn("dbo.WorkOrderMiscCost", "Version");
            DropColumn("dbo.WorkOrderLabor", "Version");
            DropColumn("dbo.WorkOrderItem", "Version");
            DropColumn("dbo.ServiceRequest", "Version");
            DropColumn("dbo.Task", "Version");
            DropColumn("dbo.TaskGroup", "Version");
            DropColumn("dbo.PMTask", "Version");
            DropColumn("dbo.ServiceItem", "Version");
            DropColumn("dbo.PMServiceItem", "Version");
            DropColumn("dbo.PMMiscCost", "Version");
            DropColumn("dbo.ShiftPattern", "Version");
            DropColumn("dbo.Shift", "Version");
            DropColumn("dbo.CalendarNonWorking", "Version");
            DropColumn("dbo.Calendar", "Version");
            DropColumn("dbo.Technician", "Version");
            DropColumn("dbo.Team", "Version");
            DropColumn("dbo.Craft", "Version");
            DropColumn("dbo.PMLabor", "Version");
            DropColumn("dbo.StoreLocatorItemLog", "Version");
            DropColumn("dbo.StoreLocatorItem", "Version");
            DropColumn("dbo.StoreLocator", "Version");
            DropColumn("dbo.StoreItem", "Version");
            DropColumn("dbo.Store", "Version");
            DropColumn("dbo.PMItem", "Version");
            DropColumn("dbo.PreventiveMaintenance", "Version");
            DropColumn("dbo.Code", "Version");
            DropColumn("dbo.WorkOrder", "Version");
            DropColumn("dbo.Reading", "Version");
            DropColumn("dbo.PointMeterLineItem", "Version");
            DropColumn("dbo.Point", "Version");
            DropColumn("dbo.Meter", "Version");
            DropColumn("dbo.MeterLineItem", "Version");
            DropColumn("dbo.MeterGroup", "Version");
            DropColumn("dbo.AssetStatusHistory", "Version");
            DropColumn("dbo.UnitOfMeasure", "Version");
            DropColumn("dbo.Currency", "Version");
            DropColumn("dbo.Contact", "Version");
            DropColumn("dbo.Company", "Version");
            DropColumn("dbo.ItemGroup", "Version");
            DropColumn("dbo.Item", "Version");
            DropColumn("dbo.AssetSparePart", "Version");
            DropColumn("dbo.ValueItemCategory", "Version");
            DropColumn("dbo.ValueItem", "Version");
            DropColumn("dbo.Location", "Version");
            DropColumn("dbo.UserDevice", "Version");
            DropColumn("dbo.VisualFilter", "Version");
            DropColumn("dbo.Visual", "Version");
            DropColumn("dbo.Organization", "Version");
            DropColumn("dbo.Site", "Version");
            DropColumn("dbo.Filter", "Version");
            DropColumn("dbo.ReportFilter", "Version");
            DropColumn("dbo.ReportColumn", "Version");
            DropColumn("dbo.Report", "Version");
            DropColumn("dbo.Module", "Version");
            DropColumn("dbo.FeatureAction", "Version");
            DropColumn("dbo.Feature", "Version");
            DropColumn("dbo.PermissionRecord", "Version");
            DropColumn("dbo.SecurityGroup", "Version");
            DropColumn("dbo.UserPasswordHistory", "Version");
            DropColumn("dbo.LocaleStringResource", "Version");
            DropColumn("dbo.Language", "Version");
            DropColumn("dbo.Assignment", "Version");
            DropColumn("dbo.User", "Version");
            DropColumn("dbo.AssetLocationHistory", "Version");
            DropColumn("dbo.Asset", "Version");
            DropColumn("dbo.AssetDowntime", "Version");
            DropColumn("dbo.Address", "Version");
        }
    }
}
