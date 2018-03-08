/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(PMItemValidator))]
    public class PMItemModel : BaseEamEntityModel
    {
        public long? PreventiveMaintenanceId { get; set; }

        [BaseEamResourceDisplayName("Store")]
        public long? StoreId { get; set; }
        public string StoreName { get; set; }

        [BaseEamResourceDisplayName("Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }

        [BaseEamResourceDisplayName("ItemCategory")]
        public ItemCategory ItemItemCategory { get; set; }
        public string ItemItemCategoryText { get; set; }

        [BaseEamResourceDisplayName("UnitOfMeasure")]
        public string ItemUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("StoreLocator")]
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }

        [BaseEamResourceDisplayName("PMItem.UnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? UnitPrice { get; set; }

        [BaseEamResourceDisplayName("PMItem.PlanQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? PlanQuantity { get; set; }

        [BaseEamResourceDisplayName("PMItem.PlanTotal")]
        [UIHint("DecimalNullable")]
        public decimal? PlanTotal { get; set; }

        [BaseEamResourceDisplayName("PMItem.PlanToolHours")]
        [UIHint("DecimalNullable")]
        public decimal? PlanToolHours { get; set; }

        [BaseEamResourceDisplayName("PMItem.ToolRate")]
        [UIHint("DecimalNullable")]
        public decimal? ToolRate { get; set; }

    }
}