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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ContractValidator))]
    public class ContractModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Common.Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("Common.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public AssignmentPriority Priority { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public string PriorityText { get; set; }

        [BaseEamResourceDisplayName("Common.Status")]
        public string Status { get; set; }

        [BaseEamResourceDisplayName("Contract.ContractType")]
        public ContractType ContractType { get; set; }

        [BaseEamResourceDisplayName("Contract.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [BaseEamResourceDisplayName("Contract.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [BaseEamResourceDisplayName("Contract.Total")]
        [UIHint("DecimalNullable")]
        public decimal? Total { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public string SiteName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.WorkCategory")]
        public long? WorkCategoryId { get; set; }
        public string WorkCategoryName { get; set; }

        [BaseEamResourceDisplayName("WorkOrder.WorkType")]
        public long? WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }

        [BaseEamResourceDisplayName("Vendor")]
        public long? VendorId { get; set; }
        public string VendorName { get; set; }

        [BaseEamResourceDisplayName("Supervisor")]
        public long? SupervisorId { get; set; }
        public string SupervisorName { get; set; }

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