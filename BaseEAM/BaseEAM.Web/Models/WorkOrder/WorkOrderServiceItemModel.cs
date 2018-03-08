/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(WorkOrderServiceItemValidator))]
    public class WorkOrderServiceItemModel : BaseEamEntityModel
    {
        public long? WorkOrderId { get; set; }

        [BaseEamResourceDisplayName("ServiceItem")]
        public long? ServiceItemId { get; set; }
        public string ServiceItemName { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.PlanUnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? PlanUnitPrice { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.PlanQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? PlanQuantity { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.PlanTotal")]
        [UIHint("DecimalNullable")]
        public decimal? PlanTotal { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.ActualUnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? ActualUnitPrice { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.ActualQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? ActualQuantity { get; set; }

        [BaseEamResourceDisplayName("WorkOrderServiceItem.ActualTotal")]
        [UIHint("DecimalNullable")]
        public decimal? ActualTotal { get; set; }
    }
}