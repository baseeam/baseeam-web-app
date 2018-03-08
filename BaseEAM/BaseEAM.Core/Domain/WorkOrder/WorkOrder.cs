/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class WorkOrder : WorkflowBaseEntity, IHierarchy, ISyncEntity
    {
        public WorkOrder()
        {
            SyncId = Guid.NewGuid().ToString();
        }
        public string SyncId { get; set; }

        public string HierarchyIdPath { get; set; }
        public string HierarchyNamePath { get; set; }

        public long? ParentId { get; set; }
        public virtual WorkOrder Parent { get; set; }

        private ICollection<WorkOrder> _children;
        public virtual ICollection<WorkOrder> Children
        {
            get { return _children ?? (_children = new List<WorkOrder>()); }
            protected set { _children = value; }
        }

        public long? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }

        public long? WorkCategoryId { get; set; }
        public virtual ValueItem WorkCategory { get; set; }

        public long? WorkTypeId { get; set; }
        public virtual ValueItem WorkType { get; set; }

        public long? FailureGroupId { get; set; }
        public virtual Code FailureGroup { get; set; }

        public long? ServiceRequestId { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }

        public long? PreventiveMaintenanceId { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }

        public string RequestorName { get; set; }
        public string RequestorEmail { get; set; }
        public string RequestorPhone { get; set; }
        public DateTime? RequestedDateTime { get; set; }

        public long? SupervisorId { get; set; }
        public virtual User Supervisor { get; set; }

        public long? TaskGroupId { get; set; }
        public virtual TaskGroup TaskGroup { get; set; }

        public long? ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public DateTime? ExpectedStartDateTime { get; set; }
        public DateTime? DueDateTime { get; set; }

        public DateTime? OnSiteDateTime { get; set; }

        public DateTime? ActualStartDateTime { get; set; }
        public DateTime? ActualEndDateTime { get; set; }

        public long? ActualFailureGroupId { get; set; }
        public virtual Code ActualFailureGroup { get; set; }

        public long? ActualProblemId { get; set; }
        public virtual Code ActualProblem { get; set; }

        public long? ActualCauseId { get; set; }
        public virtual Code ActualCause { get; set; }

        public long? ResolutionId { get; set; }
        public virtual Code Resolution { get; set; }
        public string ResolutionNotes { get; set; }

        public bool SLAEnabled { get; set; }

        private ICollection<WorkOrderLabor> _workOrderLabors;
        public virtual ICollection<WorkOrderLabor> WorkOrderLabors
        {
            get { return _workOrderLabors ?? (_workOrderLabors = new List<WorkOrderLabor>()); }
            protected set { _workOrderLabors = value; }
        }

        private ICollection<WorkOrderTask> _workOrderTasks;
        public virtual ICollection<WorkOrderTask> WorkOrderTasks
        {
            get { return _workOrderTasks ?? (_workOrderTasks = new List<WorkOrderTask>()); }
            protected set { _workOrderTasks = value; }
        }

        private ICollection<WorkOrderItem> _workOrderItems;
        public virtual ICollection<WorkOrderItem> WorkOrderItems
        {
            get { return _workOrderItems ?? (_workOrderItems = new List<WorkOrderItem>()); }
            protected set { _workOrderItems = value; }
        }

        private ICollection<WorkOrderServiceItem> _workOrderServiceItems;
        public virtual ICollection<WorkOrderServiceItem> WorkOrderServiceItems
        {
            get { return _workOrderServiceItems ?? (_workOrderServiceItems = new List<WorkOrderServiceItem>()); }
            protected set { _workOrderServiceItems = value; }
        }

        private ICollection<WorkOrderMiscCost> _workOrderMiscCosts;
        public virtual ICollection<WorkOrderMiscCost> WorkOrderMiscCosts
        {
            get { return _workOrderMiscCosts ?? (_workOrderMiscCosts = new List<WorkOrderMiscCost>()); }
            protected set { _workOrderMiscCosts = value; }
        }

        public override string AssignmentType
        {
            get
            {
                return EntityType.WorkOrder;
            }
        }
    }
}
