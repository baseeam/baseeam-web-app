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
    [Validator(typeof(WorkOrderItemValidator))]
    public class WorkOrderItemModel : BaseEamEntityModel
    {
        public long? WorkOrderId { get; set; }

        [BaseEamResourceDisplayName("Store")]
        public long? StoreId { get; set; }
        public string StoreName { get; set; }

        [BaseEamResourceDisplayName("Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }

        [BaseEamResourceDisplayName("ItemCategory")]
        public ItemCategory ItemItemCategory { get; set; }
        public string ItemItemCategoryText { get; set; }

        [BaseEamResourceDisplayName("Item.UnitOfMeasure")]
        public string ItemUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("StoreLocator")]
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.UnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? UnitPrice { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.PlanQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? PlanQuantity { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.PlanTotal")]
        [UIHint("DecimalNullable")]
        public decimal? PlanTotal { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.ActualQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? ActualQuantity { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.ActualTotal")]
        [UIHint("DecimalNullable")]
        public decimal? ActualTotal { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.PlanToolHours")]
        [UIHint("DecimalNullable")]
        public decimal? PlanToolHours { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.ToolRate")]
        [UIHint("DecimalNullable")]
        public decimal? ToolRate { get; set; }

        [BaseEamResourceDisplayName("WorkOrderItem.ActualToolHours")]
        [UIHint("DecimalNullable")]
        public decimal? ActualToolHours { get; set; }

    }
}