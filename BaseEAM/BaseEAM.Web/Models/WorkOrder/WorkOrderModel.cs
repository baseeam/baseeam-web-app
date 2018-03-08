/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(WorkOrderValidator))]
    public class WorkOrderModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("WorkOrder.Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public AssignmentPriority Priority { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public string PriorityText { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.Status")]
        public string Status { get; set; }

        [BaseEamResourceDisplayName("Parent")]
        public long? ParentId { get; set; }

        [BaseEamResourceDisplayName("Parent")]
        public string ParentNumber { get; set; }

        public string HierarchyIdPath { get; set; }

        [BaseEamResourceDisplayName("Common.HierarchyNamePath")]
        public string HierarchyNamePath { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public string SiteName { get; set; }

        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public string AssetName { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }
        public string LocationName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.WorkCategory")]
        public long? WorkCategoryId { get; set; }
        public string WorkCategoryName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.WorkType")]
        public long? WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.FailureGroup")]
        public long? FailureGroupId { get; set; }
        public string FailureGroupName { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest")]
        public long? ServiceRequestId { get; set; }
        public string ServiceRequestNumber { get; set; }

        [BaseEamResourceDisplayName("PreventiveMaintenance")]
        public long? PreventiveMaintenanceId { get; set; }
        public string PreventiveMaintenanceNumber { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.RequestorName")]
        public string RequestorName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.RequestorEmail")]
        public string RequestorEmail { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.RequestorPhone")]
        public string RequestorPhone { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.RequestedDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? RequestedDateTime { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.Supervisor")]
        public long? SupervisorId { get; set; }
        public string SupervisorName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.ExpectedStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? ExpectedStartDateTime { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.DueDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? DueDateTime { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.OnSiteDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? OnSiteDateTime { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.SLAEnabled")]
        public bool SLAEnabled { get; set; }

        //Work Order Actual
        [BaseEamResourceDisplayName("WorkOrder.ActualStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? ActualStartDateTime { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.ActualEndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? ActualEndDateTime { get; set; }

        [BaseEamResourceDisplayName("ActualFailureGroup")]
        public long? ActualFailureGroupId { get; set; }
        public string ActualFailureGroupName { get; set; }

        [BaseEamResourceDisplayName("ActualProblem")]
        public long? ActualProblemId { get; set; }
        public string ActualProblemName { get; set; }

        [BaseEamResourceDisplayName("ActualCause")]
        public long? ActualCauseId { get; set; }
        public string ActualCauseName { get; set; }

        [BaseEamResourceDisplayName("Resolution")]
        public long? ResolutionId { get; set; }
        public string ResolutionName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.ResolutionNotes")]
        public string ResolutionNotes { get; set; }

        [BaseEamResourceDisplayName("TaskGroup")]
        public long? TaskGroupId { get; set; }

        [BaseEamResourceDisplayName("Contract")]
        public long? ContractId { get; set; }

        /// <summary>
        /// Cache available actions from assignment
        /// </summary>
        public string AvailableActions { get; set; }

        [BaseEamResourceDisplayName("Common.AssignedUsers")]
        public string AssignedUsers { get; set; }

        public string Comment { get; set; }
        public string ActionName { get; set; }
    }
}