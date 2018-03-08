/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data
{
    public static class MySqlTemplate
    {
        public static readonly string ContractSearch =
            "SELECT DISTINCT Contract.Id, Contract.Number, Contract.Description, Contract.Priority," +
            " Contract.StartDate, Contract.EndDate, Contract.Total, Site.Id, Site.Name," +
            " Company.Id, Company.Name, Assignment.Id, Assignment.Name FROM Contract" +
            " LEFT JOIN Assignment ON Assignment.Id = Contract.AssignmentId" +
            " INNER JOIN Company ON Contract.VendorId = Company.Id" +
            " INNER JOIN Site ON Contract.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Contract", "Company", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ContractSearchCount =
            "SELECT COUNT(DISTINCT Contract.Id) FROM Contract" +
            " LEFT JOIN Assignment ON Assignment.Id = Contract.AssignmentId" +
            " INNER JOIN Company ON Contract.VendorId = Company.Id" +
            " INNER JOIN Site ON Contract.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Contract", "Company", "Site", "User");

        public static readonly string TenantPaymentSearch =
          "SELECT DISTINCT TenantPayment.Id, TenantPayment.Name, TenantPayment.DueDate, TenantPayment.DueAmount," +
          " TenantPayment.DaysLateCount, TenantPayment.LateFeeAmount, TenantPayment.CollectedAmount, TenantPayment.BalanceAmount," +
          " Site.Id, Site.Name, Tenant.Id, Tenant.Name, Property.Id, Property.Name, TenantLease.Id, TenantLease.Number," +
          " ChargeType.Id, ChargeType.Name FROM TenantPayment" +
          " INNER JOIN Tenant ON Tenant.Id = TenantPayment.TenantId" +
          " INNER JOIN TenantLease ON TenantLease.Id = TenantPayment.TenantLeaseId" +
          " INNER JOIN ValueItem AS ChargeType ON TenantPayment.ChargeTypeId = ChargeType.Id" +
          " INNER JOIN Property ON Property.Id = TenantPayment.PropertyId" +
          " INNER JOIN Site ON TenantPayment.SiteId = Site.Id" +
          " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
          " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
          " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
          " /**where**/" +
          ActiveCondition("TenantPayment", "Site", "Tenant", "ChargeType", "Property", "TenantLease") +
          " /**orderby**/" +
          " LIMIT @skip, @take";

        public static readonly string TenantPaymentSearchCount =
          "SELECT COUNT(DISTINCT TenantPayment.Id) FROM TenantPayment" +
          " INNER JOIN Tenant ON Tenant.Id = TenantPayment.TenantId" +
          " INNER JOIN TenantLease ON TenantLease.Id = TenantPayment.TenantLeaseId" +
          " INNER JOIN ValueItem AS ChargeType ON TenantPayment.ChargeTypeId = ChargeType.Id" +
          " INNER JOIN Property ON Property.Id = TenantPayment.PropertyId" +
          " INNER JOIN Site ON TenantPayment.SiteId = Site.Id" +
          " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
          " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
          " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
          " /**where**/" +
          ActiveCondition("TenantPayment", "Site", "Tenant", "ChargeType", "Property", "TenantLease");

        public static readonly string TenantSearch =
            "SELECT * FROM Tenant" +
            " /**where**/" +
            ActiveCondition("Tenant") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string TenantSearchCount =
            "SELECT COUNT(1) FROM Tenant" +
            " /**where**/" +
            ActiveCondition("Tenant");


        public static readonly string PropertySearch =
           "SELECT DISTINCT Property.Id, Property.Name, Site.Id, Site.Name, Location.Id, Location.Name FROM Property" +
           " INNER JOIN Location ON Location.Id = Property.LocationId" +
           " INNER JOIN Site ON Property.SiteId = Site.Id" +
           " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
           " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
           " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
           " /**where**/" +
           ActiveCondition("Property", "Site", "Location") +
           " /**orderby**/" +
           " LIMIT @skip, @take";

        public static readonly string PropertySearchCount =
            "SELECT COUNT(DISTINCT Property.Id) FROM Property" +
            " INNER JOIN Location ON Location.Id = Property.LocationId" +
            " INNER JOIN Site ON Property.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Property", "Site", "Location");


        public static readonly string TenantLeaseSearch =
            "SELECT DISTINCT TenantLease.Id, TenantLease.Number, TenantLease.Description, TenantLease.Priority," +
            " TenantLease.TermStartDate, TenantLease.TermEndDate, Site.Id, Site.Name," +
            " Tenant.Id, Tenant.Name, Property.Id, Property.Name, Assignment.Id, Assignment.Name FROM TenantLease" +
            " LEFT JOIN Assignment ON Assignment.Id = TenantLease.AssignmentId" +
            " INNER JOIN Site ON TenantLease.SiteId = Site.Id" +
            " INNER JOIN Tenant ON TenantLease.TenantId = Tenant.Id" +
            " INNER JOIN Property ON TenantLease.PropertyId = Property.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("TenantLease", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string TenantLeaseSearchCount =
            "SELECT COUNT(DISTINCT TenantLease.Id) FROM TenantLease" +
            " LEFT JOIN Assignment ON Assignment.Id = TenantLease.AssignmentId" +
            " INNER JOIN Site ON TenantLease.SiteId = Site.Id" +
            " INNER JOIN Tenant ON TenantLease.TenantId = Tenant.Id" +
            " INNER JOIN Property ON TenantLease.PropertyId = Property.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("TenantLease", "Site", "User");

        public static readonly string RequestForQuotationSearch =
            "SELECT DISTINCT RequestForQuotation.Id, RequestForQuotation.Number, RequestForQuotation.Description, RequestForQuotation.Priority," +
            " RequestForQuotation.ExpectedQuoteDate, RequestForQuotation.DateRequired," +
            " Site.Id, Site.Name, Assignment.Id, Assignment.Name FROM RequestForQuotation" +
            " LEFT JOIN Assignment ON Assignment.Id = RequestForQuotation.AssignmentId" +
            " INNER JOIN Site ON RequestForQuotation.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("RequestForQuotation", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string RequestForQuotationSearchCount =
            "SELECT COUNT(DISTINCT RequestForQuotation.Id) FROM RequestForQuotation" +
            " LEFT JOIN Assignment ON Assignment.Id = RequestForQuotation.AssignmentId" +
            " INNER JOIN Site ON RequestForQuotation.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("RequestForQuotation", "Site", "User");

        public static readonly string PurchaseRequestSearch =
            "SELECT DISTINCT PurchaseRequest.Id, PurchaseRequest.Number, PurchaseRequest.Description, PurchaseRequest.Priority," +
            " PurchaseRequest.DateRequired, Site.Id, Site.Name, Assignment.Id, Assignment.Name FROM PurchaseRequest" +
            " LEFT JOIN Assignment ON Assignment.Id = PurchaseRequest.AssignmentId" +
            " INNER JOIN Site ON PurchaseRequest.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("PurchaseRequest", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string PurchaseRequestSearchCount =
            "SELECT COUNT(DISTINCT PurchaseRequest.Id) FROM PurchaseRequest" +
            " LEFT JOIN Assignment ON Assignment.Id = PurchaseRequest.AssignmentId" +
            " INNER JOIN Site ON PurchaseRequest.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("PurchaseRequest", "Site", "User");

        public static readonly string PurchaseOrderSearch =
            "SELECT DISTINCT PurchaseOrder.Id, PurchaseOrder.Number, PurchaseOrder.Description, PurchaseOrder.Priority," +
            " PurchaseOrder.ExpectedDeliveryDate, PurchaseOrder.DateOrdered, PurchaseOrder.DateRequired," +
            " Company.Id, Company.Name, ValueItem.Id, ValueItem.Name," +
            " Site.Id, Site.Name, Assignment.Id, Assignment.Name FROM PurchaseOrder" +
            " LEFT JOIN Company ON Company.Id = PurchaseOrder.VendorId" +
            " LEFT JOIN ValueItem ON ValueItem.Id = PurchaseOrder.PaymentTermId" +
            " LEFT JOIN Assignment ON Assignment.Id = PurchaseOrder.AssignmentId" +
            " INNER JOIN Site ON PurchaseOrder.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("PurchaseOrder", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string PurchaseOrderSearchCount =
            "SELECT COUNT(DISTINCT PurchaseOrder.Id) FROM PurchaseOrder" +
            " LEFT JOIN Company ON Company.Id = PurchaseOrder.VendorId" +
            " LEFT JOIN ValueItem ON ValueItem.Id = PurchaseOrder.PaymentTermId" +
            " LEFT JOIN Assignment ON Assignment.Id = PurchaseOrder.AssignmentId" +
            " INNER JOIN Site ON PurchaseOrder.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("PurchaseOrder", "Site", "User");

        public static readonly string MyAssignmentSearch =
            "SELECT DISTINCT Assignment.* FROM Assignment" +
            " INNER JOIN Assignment_User ON Assignment.Id = Assignment_User.AssignmentId" +
            " INNER JOIN User ON User.Id = Assignment_User.UserId" +
            " /**where**/" +
            ActiveCondition("Assignment") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string MyAssignmentSearchCount =
             "SELECT COUNT(DISTINCT  Assignment.Id) FROM Assignment" +
             " INNER JOIN Assignment_User ON Assignment.Id = Assignment_User.AssignmentId" +
             " INNER JOIN User ON User.Id = Assignment_User.UserId" +
             " /**where**/" +
             ActiveCondition("Assignment");

        public static readonly string ServiceRequestSearch =
            "SELECT DISTINCT ServiceRequest.Id, ServiceRequest.Number, ServiceRequest.Description, ServiceRequest.Priority," +
            " Asset.Id, Asset.Name, Location.Id, Location.Name, Site.Id, Site.Name, Assignment.Id, Assignment.Name FROM ServiceRequest" +
            " LEFT JOIN Asset ON Asset.Id = ServiceRequest.AssetId" +
            " LEFT JOIN Location ON Location.Id = ServiceRequest.LocationId" +
            " LEFT JOIN Assignment ON Assignment.Id = ServiceRequest.AssignmentId" +
            " INNER JOIN Site ON ServiceRequest.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("ServiceRequest", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ServiceRequestSearchCount =
            "SELECT COUNT(DISTINCT ServiceRequest.Id) FROM ServiceRequest" +
            " LEFT JOIN Asset ON Asset.Id = ServiceRequest.AssetId" +
            " LEFT JOIN Location ON Location.Id = ServiceRequest.LocationId" +
            " LEFT JOIN Assignment ON Assignment.Id = ServiceRequest.AssignmentId" +
            " INNER JOIN Site ON ServiceRequest.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("ServiceRequest", "Site", "User");

        public static readonly string PreventiveMaintenanceSearch =
           "SELECT DISTINCT PreventiveMaintenance.Id, PreventiveMaintenance.Number, PreventiveMaintenance.Description, PreventiveMaintenance.Priority, ValueItem.Id, ValueItem.Name," +
           " Asset.Id, Asset.Name, Location.Id, Location.Name, Site.Id, Site.Name FROM PreventiveMaintenance" +
           " LEFT JOIN ValueItem ON PreventiveMaintenance.PMStatusId = ValueItem.Id" +
           " LEFT JOIN Asset ON Asset.Id = PreventiveMaintenance.AssetId" +
           " LEFT JOIN Location ON Location.Id = PreventiveMaintenance.LocationId" +
           " INNER JOIN Site ON PreventiveMaintenance.SiteId = Site.Id" +
           " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
           " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
           " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
           " /**where**/" +
           ActiveCondition("PreventiveMaintenance", "Asset", "Location", "Site", "User") +
           " /**orderby**/" +
           " LIMIT @skip, @take";

        public static readonly string PreventiveMaintenanceSearchCount =
            "SELECT COUNT(DISTINCT PreventiveMaintenance.Id) FROM PreventiveMaintenance" +
            " LEFT JOIN Asset ON Asset.Id = PreventiveMaintenance.AssetId" +
            " LEFT JOIN Location ON Location.Id = PreventiveMaintenance.LocationId" +
            " INNER JOIN Site ON PreventiveMaintenance.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("PreventiveMaintenance", "Asset", "Location", "Site", "User");

        public static readonly string VisualSearch =
            "SELECT DISTINCT Visual.Id, Visual.Name, Visual.VisualType, Visual.Description FROM Visual" +
            " INNER JOIN Visual_SecurityGroup ON Visual.Id = Visual_SecurityGroup.VisualId" +
            " INNER JOIN SecurityGroup_User ON Visual_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Visual", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string VisualSearchCount =
            "SELECT COUNT(DISTINCT Visual.Id) FROM Visual" +
            " INNER JOIN Visual_SecurityGroup ON Visual.Id = Visual_SecurityGroup.VisualId" +
            " INNER JOIN SecurityGroup_User ON Visual_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Visual", "User");

        public static readonly string WorkOrderSearch =
            "SELECT DISTINCT WorkOrder.Id, WorkOrder.Number, WorkOrder.Description, WorkOrder.ExpectedStartDateTime, WorkOrder.Priority," +
            " Asset.Id, Asset.Name, Location.Id, Location.Name, Site.Id, Site.Name, Assignment.Id, Assignment.Name FROM WorkOrder" +
            " LEFT JOIN Asset ON Asset.Id = WorkOrder.AssetId" +
            " LEFT JOIN Location ON Location.Id = WorkOrder.LocationId" +
            " LEFT JOIN Assignment ON Assignment.Id = WorkOrder.AssignmentId" +
            " INNER JOIN Site ON WorkOrder.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("WorkOrder", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string WorkOrderSearchCount =
            "SELECT COUNT(DISTINCT WorkOrder.Id) FROM WorkOrder" +
            " LEFT JOIN Asset ON Asset.Id = WorkOrder.AssetId" +
            " LEFT JOIN Location ON Location.Id = WorkOrder.LocationId" +
            " LEFT JOIN Assignment ON Assignment.Id = WorkOrder.AssignmentId" +
            " INNER JOIN Site ON WorkOrder.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("WorkOrder", "Site", "User");

        public static readonly string PhysicalCountSearch =
           "SELECT DISTINCT PhysicalCount.Id, PhysicalCount.Number, PhysicalCount.PhysicalCountDate, PhysicalCount.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name FROM PhysicalCount" +
            " INNER JOIN Site ON PhysicalCount.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " INNER JOIN Store ON PhysicalCount.StoreId = Store.Id" +
            " /**where**/" +
            ActiveCondition("PhysicalCount", "Site", "User", "Store") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string PhysicalCountSearchCount =
            "SELECT COUNT( DISTINCT PhysicalCount.Id, PhysicalCount.Number, PhysicalCount.PhysicalCountDate, PhysicalCount.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name) FROM PhysicalCount" +
            " INNER JOIN Site ON PhysicalCount.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " INNER JOIN Store ON PhysicalCount.StoreId = Store.Id" +
            " /**where**/" +
            ActiveCondition("PhysicalCount", "Site", "User", "Store");

        public static readonly string GetApprovedIssueItemsSearch =
           "SELECT DISTINCT IssueItem.Id, IssueItem.IssueQuantity, Issue.Id, Issue.Number, Issue.IssueDate, Item.Id, Item.Name," +
           " UnitOfMeasure.Id, UnitOfMeasure.Name, StoreLocator.Id, StoreLocator.Name FROM IssueItem" +
           " INNER JOIN Issue ON IssueItem.IssueId = Issue.Id AND Issue.IsApproved = 1" +
           " INNER JOIN Item ON IssueItem.ItemId = Item.Id" +
           " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
           " INNER JOIN StoreLocator ON IssueItem.StoreLocatorId = StoreLocator.Id" +
           " INNER JOIN Store ON StoreLocator.StoreId = Store.Id" +
           " /**where**/" +
           ActiveCondition("IssueItem", "Issue", "Item", "UnitOfMeasure", "StoreLocator", "Store") +
           " /**orderby**/" +
           " LIMIT @skip, @take";

        public static readonly string GetApprovedIssueItemsSearchCount =
           "SELECT COUNT( DISTINCT IssueItem.Id, IssueItem.IssueQuantity, Issue.Id, Issue.Number, Issue.IssueDate, Item.Id, Item.Name," +
           " UnitOfMeasure.Id, UnitOfMeasure.Name, StoreLocator.Id, StoreLocator.Name) FROM IssueItem" +
           " INNER JOIN Issue ON IssueItem.IssueId = Issue.Id AND Issue.IsApproved = 1" +
           " INNER JOIN Item ON IssueItem.ItemId = Item.Id" +
           " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
           " INNER JOIN StoreLocator ON IssueItem.StoreLocatorId = StoreLocator.Id" +
           " INNER JOIN Store ON StoreLocator.StoreId = Store.Id" +
           " /**where**/" +
           ActiveCondition("IssueItem", "Issue", "Item", "UnitOfMeasure", "StoreLocator", "Store");

        public static readonly string ReturnSearch =
            "SELECT DISTINCT Return.Id, Return.Number, Return.Description, Return.ReturnDate, Return.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name FROM `Return`" +
            " INNER JOIN Store ON Store.Id = Return.StoreId" +
            " INNER JOIN Site ON Return.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Return", "Store", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ReturnSearchCount =
            "SELECT COUNT(DISTINCT Return.Id, Return.Number, Return.Description, Return.ReturnDate, Return.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name) FROM `Return`" +
            " INNER JOIN Store ON Store.Id = Return.StoreId" +
            " INNER JOIN Site ON Return.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Return", "Store", "Site", "User");

        public static readonly string StoreLocatorItemLogSearch =
            "SELECT StoreLocatorItemLog.*, StoreLocator.Id, StoreLocator.Name, Item.Id, Item.Name FROM StoreLocatorItemLog" +
            " INNER JOIN StoreLocator ON StoreLocator.Id = StoreLocatorItemLog.StoreLocatorId" +
            " INNER JOIN Item ON Item.Id = StoreLocatorItemLog.ItemId" +
            " /**where**/" +
            ActiveCondition("StoreLocatorItemLog", "StoreLocator", "Item") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string StoreLocatorItemLogSearchCount =
             "SELECT COUNT(1) FROM StoreLocatorItemLog" +
            " INNER JOIN StoreLocator ON StoreLocator.Id = StoreLocatorItemLog.StoreLocatorId" +
            " INNER JOIN Item ON Item.Id = StoreLocatorItemLog.ItemId" +
            " /**where**/" +
            ActiveCondition("StoreLocatorItemLog", "StoreLocator", "Item");

        public static readonly string StoreLocatorItemSearch =
            "SELECT DISTINCT A.StoreId, A.StoreLocatorId, A.ItemId, A.TotalQuantity, A.TotalCost, Site.Id AS SiteId, Site.Name AS SiteName, Store.Name AS StoreName, StoreLocator.Name AS StoreLocatorName, Item.Name AS ItemName, UnitOfMeasure.Name AS ItemUnitOfMeasureName FROM" +
            " (SELECT SLI.StoreId, SLI.StoreLocatorId, SLI.ItemId, SUM(SLI.Quantity) AS TotalQuantity, SUM(SLI.Cost) AS TotalCost" +
            " FROM StoreLocatorItem AS SLI" +
            " GROUP BY SLI.StoreId, SLI.StoreLocatorId, SLI.ItemId) A" +
            " INNER JOIN Item ON Item.Id = A.ItemId" +
            " INNER JOIN UnitOfMeasure ON UnitOfMeasure.Id = Item.UnitOfMeasureId" +
            " INNER JOIN Store ON Store.Id = A.StoreId" +
            " INNER JOIN StoreLocator ON StoreLocator.Id = A.StoreLocatorId" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Item", "UnitOfMeasure", "Store", "StoreLocator", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string StoreLocatorItemSearchCount =
             "SELECT COUNT(DISTINCT A.StoreId, A.StoreLocatorId, A.ItemId, A.TotalQuantity, A.TotalCost, Site.Id, Site.Name, Store.Name, StoreLocator.Name, Item.Name, UnitOfMeasure.Name) FROM" +
            " (SELECT SLI.StoreId, SLI.StoreLocatorId, SLI.ItemId, SUM(SLI.Quantity) AS TotalQuantity, SUM(SLI.Cost) AS TotalCost" +
            " FROM StoreLocatorItem AS SLI" +
            " GROUP BY SLI.StoreId, SLI.StoreLocatorId, SLI.ItemId) A" +
            " INNER JOIN Item ON Item.Id = A.ItemId" +
            " INNER JOIN UnitOfMeasure ON UnitOfMeasure.Id = Item.UnitOfMeasureId" +
            " INNER JOIN Store ON Store.Id = A.StoreId" +
            " INNER JOIN StoreLocator ON StoreLocator.Id = A.StoreLocatorId" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Item", "UnitOfMeasure", "Store", "StoreLocator", "Site", "User");

        public static readonly string AdjustSearch =
           "SELECT DISTINCT Adjust.Id, Adjust.Number, Adjust.AdjustDate, Adjust.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name FROM Adjust" +
            " INNER JOIN Site ON Adjust.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " INNER JOIN Store ON Adjust.StoreId = Store.Id" +
            " /**where**/" +
            ActiveCondition("Adjust", "Site", "User", "Store") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string AdjustSearchCount =
            "SELECT COUNT( DISTINCT Adjust.Id, Adjust.Number, Adjust.AdjustDate, Adjust.IsApproved," +
            " Site.Id, Site.Name, Store.Id, Store.Name) FROM Adjust" +
            " INNER JOIN Site ON Adjust.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " INNER JOIN Store ON Adjust.StoreId = Store.Id" +
            " /**where**/" +
            ActiveCondition("Adjust", "Site", "User", "Store");

        public static readonly string LowInventoryStoreItemsSearch =
            "SELECT DISTINCT StoreItem.StoreId, StoreItem.ItemId, A.TotalQuantity, Site.Id AS SiteId, StoreItem.Id AS StoreItemId," +
            " StoreItem.SafetyStock, StoreItem.ReorderPoint, StoreItem.EconomicOrderQuantity, PurchaseRequest.Id AS PurchaseRequestId" +
            " FROM StoreItem LEFT JOIN" +
            " (SELECT SLI.StoreId, SLI.ItemId, SUM(SLI.Quantity) AS TotalQuantity" +
            " FROM StoreLocatorItem AS SLI" +
            " GROUP BY SLI.StoreId, SLI.ItemId) A" +
            " ON StoreItem.StoreId = A.StoreId AND StoreItem.ItemId = A.ItemId" +
            " INNER JOIN Item ON Item.Id = StoreItem.ItemId" +
            " INNER JOIN Store ON Store.Id = StoreItem.StoreId" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " LEFT JOIN PurchaseRequest ON PurchaseRequest.Id = StoreItem.PurchaseRequestId " +
            " LEFT JOIN Assignment ON PurchaseRequest.AssignmentId = Assignment.Id" +
            " WHERE StoreItem.ReorderPoint IS NOT NULL AND StoreItem.ReorderPoint != '' AND StoreItem.ReorderPoint > A.TotalQuantity" +
            " AND (PurchaseRequest.Id IS NULL OR Assignment.Name = 'Approved' OR Assignment.Name = 'Rejected')" +
            ActiveCondition("Item", "Store", "StoreItem", "Site");

        public static readonly string StoreItemSearch =
            "SELECT DISTINCT StoreItem.StoreId, StoreItem.ItemId, A.TotalQuantity, A.TotalCost, Site.Id AS SiteId, Site.Name AS SiteName, Store.Name AS StoreName, Item.Name AS ItemName, StoreItem.Id AS StoreItemId, StoreItem.StockType AS StoreItemStockType, UnitOfMeasure.Name AS ItemUnitOfMeasureName" +
            " FROM StoreItem LEFT JOIN" +
            " (SELECT SLI.StoreId, SLI.ItemId, SUM(SLI.Quantity) AS TotalQuantity, SUM(SLI.Cost) AS TotalCost" +
            " FROM StoreLocatorItem AS SLI" +
            " GROUP BY SLI.StoreId, SLI.ItemId) A" +
            " ON StoreItem.StoreId = A.StoreId AND StoreItem.ItemId = A.ItemId" +
            " INNER JOIN Item ON Item.Id = StoreItem.ItemId" +
            " INNER JOIN UnitOfMeasure ON UnitOfMeasure.Id = Item.UnitOfMeasureId" +
            " INNER JOIN Store ON Store.Id = StoreItem.StoreId" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Item", "UnitOfMeasure", "Store", "StoreItem", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string StoreItemSearchCount =
             "SELECT COUNT(DISTINCT StoreItem.StoreId, StoreItem.ItemId)" +
            " FROM StoreItem LEFT JOIN" + 
            " (SELECT SLI.StoreId, SLI.ItemId, SUM(SLI.Quantity) AS TotalQuantity, SUM(SLI.Cost) AS TotalCost" +
            " FROM StoreLocatorItem AS SLI" +
            " GROUP BY SLI.StoreId, SLI.ItemId) A" +
            " ON StoreItem.StoreId = A.StoreId AND StoreItem.ItemId = A.ItemId" +
            " INNER JOIN Item ON Item.Id = StoreItem.ItemId" +
            " INNER JOIN UnitOfMeasure ON UnitOfMeasure.Id = Item.UnitOfMeasureId" +
            " INNER JOIN Store ON Store.Id = StoreItem.StoreId" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Item", "UnitOfMeasure", "Store", "StoreItem", "Site", "User");

        public static readonly string TransferSearch =
           "SELECT DISTINCT Transfer.Id, Transfer.Number, Transfer.Description, Transfer.TransferDate, Transfer.IsApproved," +
            " FromSite.Id, FromSite.Name, ToSite.Id, ToSite.Name, FromStore.Id, FromStore.Name, ToStore.Id, ToStore.Name FROM Transfer" +
            " INNER JOIN Site AS FromSite ON Transfer.FromSiteId = FromSite.Id" +
            " INNER JOIN Site_SecurityGroup AS FromSite_SecurityGroup ON FromSite.Id = FromSite_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User AS FromSite_SecurityGroup_User ON FromSite_SecurityGroup.SecurityGroupId = FromSite_SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User AS FromSite_User ON FromSite_SecurityGroup_User.UserId = FromSite_User.Id" +
            " INNER JOIN Site AS ToSite ON Transfer.ToSiteId = ToSite.Id" +
            " INNER JOIN Site_SecurityGroup AS ToSite_SecurityGroup ON FromSite.Id = ToSite_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User AS ToSite_SecurityGroup_User ON ToSite_SecurityGroup.SecurityGroupId = ToSite_SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User AS ToSite_User ON ToSite_SecurityGroup_User.UserId = ToSite_User.Id" +
            " INNER JOIN Store AS FromStore ON Transfer.FromStoreId = FromStore.Id" +
            " INNER JOIN Store AS ToStore ON Transfer.ToStoreId = ToStore.Id" +
            " /**where**/" +
            ActiveCondition("Transfer", "FromSite", "ToSite", "FromSite_User", "ToSite_User", "FromStore", "ToStore") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string TransferSearchCount =
            "SELECT COUNT( DISTINCT Transfer.Id, Transfer.Number, Transfer.Description, Transfer.TransferDate, Transfer.IsApproved," +
            " FromSite.Id, FromSite.Name, ToSite.Id, ToSite.Name, FromStore.Id, FromStore.Name, ToStore.Id, ToStore.Name) FROM Transfer" +
            " INNER JOIN Site AS FromSite ON Transfer.FromSiteId = FromSite.Id" +
            " INNER JOIN Site_SecurityGroup AS FromSite_SecurityGroup ON FromSite.Id = FromSite_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User AS FromSite_SecurityGroup_User ON FromSite_SecurityGroup.SecurityGroupId = FromSite_SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User AS FromSite_User ON FromSite_SecurityGroup_User.UserId = FromSite_User.Id" +
            " INNER JOIN Site AS ToSite ON Transfer.ToSiteId = ToSite.Id" +
            " INNER JOIN Site_SecurityGroup AS ToSite_SecurityGroup ON FromSite.Id = ToSite_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User AS ToSite_SecurityGroup_User ON ToSite_SecurityGroup.SecurityGroupId = ToSite_SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User AS ToSite_User ON ToSite_SecurityGroup_User.UserId = ToSite_User.Id" +
            " INNER JOIN Store AS FromStore ON Transfer.FromStoreId = FromStore.Id" +
            " INNER JOIN Store AS ToStore ON Transfer.ToStoreId = ToStore.Id" +
            " /**where**/" +
            ActiveCondition("Transfer", "FromSite", "ToSite", "FromSite_User", "ToSite_User", "FromStore", "ToStore");

        public static readonly string ServiceItemSearch =
            "SELECT DISTINCT  ServiceItem.Id, ServiceItem.Name, ServiceItem.Description, ServiceItem.UnitPrice, ItemGroup.Id, ItemGroup.Name FROM ServiceItem" +
            " INNER JOIN ItemGroup ON ServiceItem.ItemGroupId = ItemGroup.Id" +
            " /**where**/" +
            ActiveCondition("ServiceItem", "ItemGroup") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ServiceItemSearchCount =
             "SELECT COUNT(DISTINCT  ServiceItem.Id, ServiceItem.Name, ServiceItem.Description, ItemGroup.Id, ItemGroup.Name) FROM ServiceItem" +
             " INNER JOIN ItemGroup ON ServiceItem.ItemGroupId = ItemGroup.Id" +
             " /**where**/" +
             ActiveCondition("ServiceItem", "ItemGroup");

        public static readonly string IssueSearch =
            "SELECT DISTINCT Issue.Id, Issue.Number, Issue.Description, Issue.IssueDate, Issue.IsApproved, Site.Id, Site.Name, Store.Id, Store.Name FROM Issue" +
            " INNER JOIN Store ON Store.Id = Issue.StoreId" +
            " INNER JOIN Site ON Issue.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Issue", "Store", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string IssueSearchCount =
            "SELECT COUNT(DISTINCT Issue.Id, Issue.Number, Issue.Description, Issue.IssueDate, Issue.IsApproved, Site.Id, Site.Name, Store.Id, Store.Name) FROM Issue" +
            " INNER JOIN Store ON Store.Id = Issue.StoreId" +
            " INNER JOIN Site ON Issue.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Issue", "Store", "Site", "User");

        public static readonly string TechnicianSearch =
           "SELECT DISTINCT Technician.Id, Technician.Name, User.Id, User.Name, Calendar.Id, Calendar.Name, Shift.Id, Shift.Name, Craft.Id, Craft.Name  FROM Technician" +
           " INNER JOIN User ON Technician.UserId = User.Id" +
           " LEFT JOIN Team_Technician ON Technician.Id = Team_Technician.TechnicianId" +
           " LEFT JOIN Team ON Team.Id = Team_Technician.TeamId" +
           " INNER JOIN Calendar ON Technician.CalendarId = Calendar.Id" +
           " INNER JOIN Shift ON Technician.ShiftId = Shift.Id" +
           " INNER JOIN Craft ON Technician.CraftId = Craft.Id" +
           " /**where**/" +
           ActiveCondition("Technician", "User", "Calendar", "Shift", "Craft") +
           " /**orderby**/" +
           " LIMIT @skip, @take";

        public static readonly string TechnicianSearchCount =
           "SELECT COUNT( DISTINCT Technician.Id, Technician.Name, User.Id, User.Name, Calendar.Id, Calendar.Name, Shift.Id, Shift.Name, Craft.Id, Craft.Name) FROM Technician" +
           " INNER JOIN User ON Technician.UserId = User.Id" +
           " LEFT JOIN Team_Technician ON Technician.Id = Team_Technician.TechnicianId" +
           " LEFT JOIN Team ON Team.Id = Team_Technician.TeamId" +
           " INNER JOIN Calendar ON Technician.CalendarId = Calendar.Id" +
           " INNER JOIN Shift ON Technician.ShiftId = Shift.Id" +
           " INNER JOIN Craft ON Technician.CraftId = Craft.Id" +
           " /**where**/" +
           ActiveCondition("Technician", "User", "Calendar", "Shift", "Craft");

        public static readonly string TeamSearch =
            "SELECT DISTINCT  Team.Id, Team.Name, Team.Description,Site.Id, Site.Name FROM Team" +
            " INNER JOIN Site ON Team.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Team", "Site") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string TeamSearchCount =
             "SELECT COUNT(DISTINCT  Team.Id, Team.Name, Team.Description, Site.Id, Site.Name) FROM Team" +
             " INNER JOIN Site ON Team.SiteId = Site.Id" +
             " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
             " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
             " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
             " /**where**/" +
             ActiveCondition("Team", "Site");

        public static readonly string ReceiptSearch =
            "SELECT DISTINCT Receipt.Id, Receipt.Number, Receipt.Description, Receipt.ReceiptDate, Receipt.IsApproved,Site.Id, Site.Name, Store.Id, Store.Name FROM Receipt" +
            " INNER JOIN Store ON Store.Id = Receipt.StoreId" +
            " INNER JOIN Site ON Receipt.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Receipt", "Store", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ReceiptSearchCount =
            "SELECT COUNT(DISTINCT Receipt.Id, Receipt.Number, Receipt.Description, Receipt.ReceiptDate, Receipt.IsApproved, Site.Id, Site.Name, Store.Id, Store.Name) FROM Receipt" +
            " INNER JOIN Store ON Store.Id = Receipt.StoreId" +
            " INNER JOIN Site ON Receipt.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Receipt", "Store", "Site", "User");

        public static readonly string ShiftSearch =
            "SELECT * FROM Shift" +
            " INNER JOIN Calendar ON Shift.CalendarId = Calendar.Id" +
            " /**where**/" +
            ActiveCondition("Shift", "Calendar") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ShiftSearchCount =
             "SELECT COUNT(1) FROM Shift" +
             " INNER JOIN Calendar ON Shift.CalendarId = Calendar.Id" +
             " /**where**/" +
             ActiveCondition("Shift", "Calendar");

        public static readonly string StoreSearch =
            "SELECT DISTINCT Store.Id, Store.Name, StoreType.Id, StoreType.Name, Site.Id, Site.Name, Location.Id, Location.HierarchyNamePath FROM Store" +
            " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " LEFT JOIN Location ON Store.LocationId = Location.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Store", "StoreType", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string StoreSearchCount =
            "SELECT COUNT(DISTINCT Store.Id, Store.Name, StoreType.Id, StoreType.Name, Site.Id, Site.Name, Location.Id, Location.HierarchyNamePath) FROM Store" +
            " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
            " INNER JOIN Site ON Store.SiteId = Site.Id" +
            " LEFT JOIN Location ON Store.LocationId = Location.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Store", "StoreType", "Site", "User");

        public static readonly string AssetSearch =
            "SELECT DISTINCT Asset.Id, Asset.Name, Asset.SerialNumber, AssetType.Id, AssetType.Name, AssetStatus.Id, AssetStatus.Name, " +
            "Location.Id, Location.Name, Site.Id, Site.Name " +
            " FROM Asset" +
            " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
            " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
            " LEFT JOIN Location  ON Asset.LocationId = Location.Id" +
            " INNER JOIN Site ON Asset.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string AssetSearchCount =
            "SELECT COUNT(DISTINCT Asset.Id) FROM Asset" +
            " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
            " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
            " LEFT JOIN Location  ON Asset.LocationId = Location.Id" +
            " INNER JOIN Site ON Asset.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
             " /**where**/" +
             ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site");

        public static readonly string CompanySearch =
            "SELECT * FROM Company" +
            " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
            " /**where**/" +
            ActiveCondition("Company", "ValueItem") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string CompanySearchCount =
            "SELECT COUNT(1) FROM Company" +
            " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
            " /**where**/" +
            ActiveCondition("Company", "ValueItem");

        public static readonly string ItemSearch =
           "SELECT * FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
            " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
            " /**where**/" +
            ActiveCondition("Item", "ItemGroup", "UnitOfMeasure") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ItemCount =
           "SELECT COUNT(1) FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
            " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
            " /**where**/" +
            ActiveCondition("Item", "ItemGroup", "UnitOfMeasure");

        public static readonly string MeterSearch =
            "SELECT * FROM Meter" +
            " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
            " /**where**/" +
            ActiveCondition("Meter", "ValueItem") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string MeterSearchCount =
            "SELECT COUNT(1) FROM Meter" +
            " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
            " /**where**/" +
            ActiveCondition("Meter", "ValueItem");

        public static readonly string CommonSearch =
            "SELECT * FROM {0} /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0 /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string CommonCount =
            "SELECT COUNT(1) FROM {0} /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0";

        public static readonly string LogSearch =
            "SELECT * FROM Log INNER JOIN User ON Log.UserId = User.Id /**where**/" +
            ActiveCondition("Log", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string LogSearchCount =
            "SELECT COUNT(1) FROM Log INNER JOIN User ON Log.UserId = User.Id /**where**/" +
            ActiveCondition("Log", "User");

        public static readonly string ModuleSearch =
            "SELECT DISTINCT Module.* FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
            ActiveCondition("Module", "Feature") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ModuleSearchCount =
            "SELECT COUNT(DISTINCT Module.Id) FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
            ActiveCondition("Module", "Feature");

        public static readonly string ValueItemSearch =
            "SELECT DISTINCT ValueItem.Id, ValueItem.Name, ValueItemCategory.Id, ValueItemCategory.Name  FROM ValueItem" +
            " INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id" +
            " /**where**/" +
            ActiveCondition("ValueItem", "ValueItemCategory") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string ValueItemSearchCount =
            "SELECT COUNT(DISTINCT ValueItem.Id, ValueItem.Name, ValueItemCategory.Id, ValueItemCategory.Name) FROM ValueItem" +
            " INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id" +
            " /**where**/" +
            ActiveCondition("ValueItem", "ValueItemCategory");

        public static readonly string UnitConversionSearch =
            "SELECT * FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
            ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string UnitConversionCount =
           "SELECT COUNT(1) FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
            ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure");

        public static readonly string UserSearch =
            "SELECT DISTINCT User.* FROM User" +
            " LEFT JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
            " /**where**/" +
            ActiveCondition("User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string UserSearchCount =
            "SELECT COUNT(DISTINCT User.Id) FROM User" +
            " LEFT JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
            " /**where**/" +
            ActiveCondition("User");

        public static readonly string LocationSearch =
            "SELECT DISTINCT Location.Id, Location.Name, Location.HierarchyNamePath, LocationType.Id, LocationType.Name, LocationStatus.Id, LocationStatus.Name, Site.Id, Site.Name FROM Location" +
            " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
            " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
            " INNER JOIN Site ON Location.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Location", "LocationType", "LocationStatus", "Site", "User") +
            " /**orderby**/" +
            " LIMIT @skip, @take";

        public static readonly string LocationSearchCount =
            "SELECT COUNT(DISTINCT Location.Id, Location.Name, Location.HierarchyNamePath, LocationType.Id, LocationType.Name, LocationStatus.Id, LocationStatus.Name, Site.Id, Site.Name) FROM Location" +
            " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
            " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
            " INNER JOIN Site ON Location.SiteId = Site.Id" +
            " INNER JOIN Site_SecurityGroup ON Site.Id = Site_SecurityGroup.SiteId" +
            " INNER JOIN SecurityGroup_User ON Site_SecurityGroup.SecurityGroupId = SecurityGroup_User.SecurityGroupId" +
            " INNER JOIN User ON SecurityGroup_User.UserId = User.Id" +
            " /**where**/" +
            ActiveCondition("Location", "LocationType", "LocationStatus", "Site", "User");

        #region Common

        public static readonly string GetById =
            "SELECT * FROM `{0}` WHERE Id = {1}";

        public static readonly string DeleteById =
            "DELETE FROM `{0}` WHERE Id = {1}";

        public static readonly string GetByColumn =
            "SELECT * FROM {0} WHERE {1} = {2}";

        public static readonly string GetAll =
            "SELECT {0}, {1} FROM {2} WHERE IsDeleted = 0 AND IsNew = 0 AND {1} LIKE '%{3}%'";

        public static readonly string DeleteActivityLog =
            "DELETE FROM ActivityLog WHERE Comment = '/{0}/Edit/{1}'";

        #endregion

        public static string ActiveCondition(params string[] tables)
        {
            string result = "";
            foreach (string table in tables)
            {
                result = result + string.Format(" AND ({0}.Id IS NULL OR ({0}.IsDeleted = 0 AND {0}.IsNew = 0))", table);
            }
            return result;
        }
    }
}
