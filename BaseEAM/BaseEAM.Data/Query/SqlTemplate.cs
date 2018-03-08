/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
namespace BaseEAM.Data
{
    public static class SqlTemplate
    {
        #region Contract

        public static string ContractSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ContractSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ContractSearch;

            return template;
        }

        public static string ContractSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ContractSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ContractSearchCount;

            return template;
        }

        #endregion

        #region TenantPayment

        public static string TenantPaymentSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantPaymentSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantPaymentSearch;

            return template;
        }

        public static string TenantPaymentSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantPaymentSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantPaymentSearchCount;

            return template;
        }

        #endregion

        #region Tenant

        public static string TenantSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantSearch;

            return template;
        }

        public static string TenantSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantSearchCount;

            return template;
        }

        #endregion

        #region Property

        public static string PropertySearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PropertySearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PropertySearch;

            return template;
        }

        public static string PropertySearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PropertySearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PropertySearchCount;

            return template;
        }

        #endregion

        #region SLADefinition

        public static string SLADefinitionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "SLADefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "SLADefinition");

            return template;
        }

        public static string SLADefinitionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "SLADefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "SLADefinition");

            return template;
        }

        #endregion

        #region TenantLease

        public static string TenantLeaseSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantLeaseSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantLeaseSearch;

            return template;
        }

        public static string TenantLeaseSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TenantLeaseSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TenantLeaseSearchCount;

            return template;
        }

        #endregion

        #region RequestForQuotation

        public static string RequestForQuotationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.RequestForQuotationSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.RequestForQuotationSearch;

            return template;
        }

        public static string RequestForQuotationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.RequestForQuotationSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.RequestForQuotationSearchCount;

            return template;
        }

        #endregion

        #region PurchaseRequest

        public static string PurchaseRequestSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PurchaseRequestSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PurchaseRequestSearch;

            return template;
        }

        public static string PurchaseRequestSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PurchaseRequestSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PurchaseRequestSearchCount;

            return template;
        }

        #endregion

        #region PurchaseOrder

        public static string PurchaseOrderSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PurchaseOrderSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PurchaseOrderSearch;

            return template;
        }

        public static string PurchaseOrderSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PurchaseOrderSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PurchaseOrderSearchCount;

            return template;
        }

        #endregion

        #region AuditEntityConfiguration

        public static string AuditEntityConfigurationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "AuditEntityConfiguration");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "AuditEntityConfiguration");

            return template;
        }

        public static string AuditEntityConfigurationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "AuditEntityConfiguration");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "AuditEntityConfiguration");

            return template;
        }
        #endregion

        #region ImportProfile

        public static string ImportProfileSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "ImportProfile");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "ImportProfile");

            return template;
        }

        public static string ImportProfileSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "ImportProfile");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "ImportProfile");

            return template;
        }
        #endregion

        #region Assignment

        public static string MyAssignmentSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.MyAssignmentSearch, "Assignment");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.MyAssignmentSearch, "Assignment");

            return template;
        }

        public static string MyAssignmentSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.MyAssignmentSearchCount, "Assignment");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.MyAssignmentSearchCount, "Assignment");

            return template;
        }

        #endregion

        #region AutomatedAction

        public static string AutomatedActionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "AutomatedAction");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "AutomatedAction");

            return template;
        }

        public static string AutomatedActionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "AutomatedAction");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "AutomatedAction");

            return template;
        }
        #endregion

        #region ServiceRequest

        public static string ServiceRequestSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ServiceRequestSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ServiceRequestSearch;

            return template;
        }

        public static string ServiceRequestSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ServiceRequestSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ServiceRequestSearchCount;

            return template;
        }

        #endregion

        #region PreventiveMaintenance

        public static string PreventiveMaintenanceSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PreventiveMaintenanceSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PreventiveMaintenanceSearch;

            return template;
        }

        public static string PreventiveMaintenanceSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PreventiveMaintenanceSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PreventiveMaintenanceSearchCount;

            return template;
        }

        #endregion

        #region Visual

        public static string VisualSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.VisualSearch, "Visual");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.VisualSearch, "Visual");

            return template;
        }

        public static string VisualSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.VisualSearchCount, "Visual");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.VisualSearchCount, "Visual");

            return template;
        }

        #endregion

        #region TaskGroup

        public static string TaskGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "TaskGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "TaskGroup");

            return template;
        }

        public static string TaskGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "TaskGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "TaskGroup");

            return template;
        }
        #endregion

        #region Report

        public static string ReportSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Report");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Report");

            return template;
        }

        public static string ReportSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Report");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Report");

            return template;
        }
        #endregion

        #region AuditTrail

        public static string AuditTrailSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "AuditTrail");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "AuditTrail");

            return template;
        }

        public static string AuditTrailSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "AuditTrail");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "AuditTrail");

            return template;
        }
        #endregion

        #region WorkOrder

        public static string WorkOrderSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.WorkOrderSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.WorkOrderSearch;

            return template;
        }

        public static string WorkOrderSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.WorkOrderSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.WorkOrderSearchCount;

            return template;
        }

        #endregion

        #region AssignmentGroup

        public static string AssignmentGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "AssignmentGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "AssignmentGroup");

            return template;
        }

        public static string AssignmentGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "AssignmentGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "AssignmentGroup");

            return template;
        }
        #endregion

        #region Code

        public static string CodeSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Code");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Code");

            return template;
        }

        public static string CodeSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Code");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Code");

            return template;
        }

        #endregion

        #region Filter

        public static string FilterSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Filter");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Filter");

            return template;
        }

        public static string FilterSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Filter");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Filter");

            return template;
        }

        #endregion

        #region PhysicalCount

        public static string PhysicalCountSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PhysicalCountSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PhysicalCountSearch;

            return template;
        }

        public static string PhysicalCountSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.PhysicalCountSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.PhysicalCountSearchCount;

            return template;
        }

        #endregion

        #region GetApprovedIssueItems

        public static string GetApprovedIssueItemsSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.GetApprovedIssueItemsSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.GetApprovedIssueItemsSearch;

            return template;
        }

        public static string GetApprovedIssueItemsSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.GetApprovedIssueItemsSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.GetApprovedIssueItemsSearchCount;

            return template;
        }

        #endregion

        #region Return

        public static string ReturnSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ReturnSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ReturnSearch;

            return template;
        }

        public static string ReturnSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ReturnSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ReturnSearchCount;

            return template;
        }

        #endregion

        #region StoreLocatorItemLog

        public static string StoreLocatorItemLogSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreLocatorItemLogSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreLocatorItemLogSearch;

            return template;
        }

        public static string StoreLocatorItemLogSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreLocatorItemLogSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreLocatorItemLogSearchCount;

            return template;
        }

        #endregion

        #region StoreLocatorItem

        public static string StoreLocatorItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreLocatorItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreLocatorItemSearch;

            return template;
        }

        public static string StoreLocatorItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreLocatorItemSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreLocatorItemSearchCount;

            return template;
        }

        #endregion

        #region Adjust

        public static string AdjustSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.AdjustSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.AdjustSearch;

            return template;
        }

        public static string AdjustSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.AdjustSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.AdjustSearchCount;

            return template;
        }

        #endregion

        #region LowInventoryStoreItems

        public static string LowInventoryStoreItemsSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.LowInventoryStoreItemsSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.LowInventoryStoreItemsSearch;

            return template;
        }

        #endregion

        #region StoreItem

        public static string StoreItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreItemSearch;

            return template;
        }

        public static string StoreItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreItemSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreItemSearchCount;

            return template;
        }

        #endregion

        #region Transfer

        public static string TransferSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TransferSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TransferSearch;

            return template;
        }

        public static string TransferSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TransferSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TransferSearchCount;

            return template;
        }

        #endregion

        #region ServiceItem

        public static string ServiceItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ServiceItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ServiceItemSearch;

            return template;
        }

        public static string ServiceItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ServiceItemSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ServiceItemSearchCount;

            return template;
        }

        #endregion

        #region Message

        public static string MessageSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Message");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Message");

            return template;
        }

        public static string MessageSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Message");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Message");

            return template;
        }

        #endregion

        #region Issue

        public static string IssueSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.IssueSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.IssueSearch;

            return template;
        }

        public static string IssueSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.IssueSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.IssueSearchCount;

            return template;
        }

        #endregion

        #region Technician

        public static string TechnicianSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TechnicianSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TechnicianSearch;

            return template;
        }

        public static string TechnicianSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TechnicianSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TechnicianSearchCount;

            return template;
        }

        #endregion

        #region Team

        public static string TeamSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TeamSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TeamSearch;

            return template;
        }

        public static string TeamSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.TeamSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.TeamSearchCount;

            return template;
        }

        #endregion

        #region Receipt

        public static string ReceiptSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ReceiptSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ReceiptSearch;

            return template;
        }

        public static string ReceiptSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ReceiptSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ReceiptSearchCount;

            return template;
        }

        #endregion

        #region Craft

        public static string CraftSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Craft");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Craft");

            return template;
        }

        public static string CraftSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Craft");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Craft");

            return template;
        }

        #endregion

        #region Shift

        public static string ShiftSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ShiftSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ShiftSearch;

            return template;
        }

        public static string ShiftSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ShiftSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ShiftSearchCount;

            return template;
        }

        #endregion

        #region Calendar

        public static string CalendarSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Calendar");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Calendar");

            return template;
        }

        public static string CalendarSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Calendar");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Calendar");

            return template;
        }

        #endregion

        #region AutoNumber

        public static string AutoNumberSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "AutoNumber");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "AutoNumber");

            return template;
        }

        public static string AutoNumberSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "AutoNumber");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "AutoNumber");

            return template;
        }

        #endregion

        #region Store

        public static string StoreSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreSearch;

            return template;
        }

        public static string StoreSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.StoreSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.StoreSearchCount;

            return template;
        }

        #endregion

        #region Asset

        public static string AssetSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.AssetSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.AssetSearch;

            return template;
        }

        public static string AssetSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.AssetSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.AssetSearchCount;

            return template;
        }

        #endregion

        #region Company

        public static string CompanySearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.CompanySearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.CompanySearch;

            return template;
        }

        public static string CompanySearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.CompanySearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.CompanySearchCount;

            return template;
        }

        #endregion

        #region Item

        public static string ItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ItemSearch;

            return template;
        }

        public static string ItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ItemCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ItemCount;

            return template;
        }

        #endregion

        #region ItemGroup

        public static string ItemGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "ItemGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "ItemGroup");

            return template;
        }

        public static string ItemGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "ItemGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "ItemGroup");

            return template;
        }

        #endregion

        #region MessageTemplate

        public static string MessageTemplateSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "MessageTemplate");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "MessageTemplate");

            return template;
        }

        public static string MessageTemplateSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "MessageTemplate");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "MessageTemplate");

            return template;
        }

        #endregion

        #region WorkflowDefinition

        public static string WorkflowDefinitionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "WorkflowDefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "WorkflowDefinition");

            return template;
        }

        public static string WorkflowDefinitionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "WorkflowDefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "WorkflowDefinition");

            return template;
        }

        #endregion

        #region Attribute

        public static string AttributeSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Attribute");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Attribute");

            return template;
        }

        public static string AttributeSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Attribute");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Attribute");

            return template;
        }

        #endregion

        #region MeterGroup

        public static string MeterGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "MeterGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "MeterGroup");

            return template;
        }

        public static string MeterGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "MeterGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "MeterGroup");

            return template;
        }

        #endregion

        #region Meter

        public static string MeterSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.MeterSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.MeterSearch;

            return template;
        }

        public static string MeterSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.MeterSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.MeterSearchCount;

            return template;
        }

        #endregion

        #region Location

        public static string LocationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.LocationSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.LocationSearch;

            return template;
        }

        public static string LocationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.LocationSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.LocationSearchCount;

            return template;
        }

        #endregion

        #region User

        public static string UserSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.UserSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.UserSearch;

            return template;
        }

        public static string UserSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.UserSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.UserSearchCount;

            return template;
        }

        #endregion

        #region PermissionRecord

        public static string PermissionRecordSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "PermissionRecord");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "PermissionRecord");

            return template;
        }

        public static string PermissionRecordSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "PermissionRecord");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "PermissionRecord");

            return template;
        }

        #endregion

        #region SecurityGroup

        public static string SecurityGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "SecurityGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "SecurityGroup");

            return template;
        }

        public static string SecurityGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "SecurityGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "SecurityGroup");

            return template;
        }

        #endregion

        #region Organization

        public static string OrganizationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Organization");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Organization");

            return template;
        }

        public static string OrganizationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "Organization");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "Organization");

            return template;
        }

        #endregion

        #region Site

        public static string SiteSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Site");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Site");

            return template;
        }

        public static string SiteSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "UnitOfMeasure");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "UnitOfMeasure");

            return template;
        }

        #endregion

        #region Language

        public static string LanguageSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "Language");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "Language");

            return template;
        }

        #endregion

        #region Log

        public static string LogSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.LogSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.LogSearch;

            return template;
        }

        public static string LogSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.LogSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.LogSearchCount;

            return template;
        }

        #endregion

        #region Module

        public static string ModuleSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ModuleSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ModuleSearch;

            return template;
        }

        #endregion

        #region ValueItem

        public static string ValueItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ValueItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ValueItemSearch;

            return template;
        }

        public static string ValueItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.ValueItemSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.ValueItemSearchCount;

            return template;
        }
        #endregion

        #region UnitOfMeasure

        public static string UnitOfMeasureSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonSearch, "UnitOfMeasure");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonSearch, "UnitOfMeasure");

            return template;
        }

        public static string UnitOfMeasureSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServerTemplate.CommonCount, "UnitOfMeasure");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySqlTemplate.CommonCount, "UnitOfMeasure");

            return template;
        }

        #endregion

        #region UnitConversion

        public static string UnitConversionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.UnitConversionSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.UnitConversionSearch;

            return template;
        }

        public static string UnitConversionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.UnitConversionCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.UnitConversionCount;

            return template;
        }

        #endregion

        #region Common

        public static string GetById()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.GetById;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.GetById;

            return template;
        }

        public static string DeleteById()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.DeleteById;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.DeleteById;

            return template;
        }

        public static string GetByColumn()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.GetByColumn;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.GetByColumn;

            return template;
        }

        public static string GetAll()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.GetAll;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.GetAll;

            return template;
        }

        public static string DeleteActivityLog()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServerTemplate.DeleteActivityLog;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySqlTemplate.DeleteActivityLog;

            return template;
        }

        #endregion
    }
}
