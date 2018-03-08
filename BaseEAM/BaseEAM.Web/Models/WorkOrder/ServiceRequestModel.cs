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
    [Validator(typeof(ServiceRequestValidator))]
    public class ServiceRequestModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("ServiceRequest.Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.Status")]
        public string Status { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public string SiteName { get; set; }

        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public string AssetName { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public long? LocationId { get; set; }
        public string LocationName { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public AssignmentPriority Priority { get; set; }

        [BaseEamResourceDisplayName("Priority")]
        public string PriorityText { get; set; }

        [BaseEamResourceDisplayName("RequestorType")]
        public RequestorType RequestorType { get; set; }

        [BaseEamResourceDisplayName("RequestorType")]
        public string RequestorTypeText { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.RequestorName")]
        public string RequestorName { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.RequestorEmail")]
        public string RequestorEmail { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.RequestorPhone")]
        public string RequestorPhone { get; set; }

        [BaseEamResourceDisplayName("ServiceRequest.RequestedDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? RequestedDateTime { get; set; }


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