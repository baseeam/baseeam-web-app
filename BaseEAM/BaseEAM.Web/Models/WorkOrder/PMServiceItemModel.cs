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
    [Validator(typeof(PMServiceItemValidator))]
    public class PMServiceItemModel : BaseEamEntityModel
    {
        public long? PreventiveMaintenanceId { get; set; }

        [BaseEamResourceDisplayName("ServiceItem")]
        public long? ServiceItemId { get; set; }
        public string ServiceItemName { get; set; }

        [BaseEamResourceDisplayName("PMServiceItem.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("PMServiceItem.PlanUnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? PlanUnitPrice { get; set; }

        [BaseEamResourceDisplayName("PMServiceItem.PlanQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? PlanQuantity { get; set; }

        [BaseEamResourceDisplayName("PMServiceItem.PlanTotal")]
        [UIHint("DecimalNullable")]
        public decimal? PlanTotal { get; set; }

    }
}