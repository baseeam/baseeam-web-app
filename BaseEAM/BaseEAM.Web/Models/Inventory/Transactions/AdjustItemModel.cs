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
    [Validator(typeof(AdjustItemValidator))]
    public class AdjustItemModel : BaseEamEntityModel
    {
        //Cache StoreId from Adjust
        public long? StoreId { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.Adjust")]
        public long? AdjustId { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.StoreLocator")]
        public long? StoreLocatorId { get; set; }
        public StoreLocatorModel StoreLocator { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        [BaseEamResourceDisplayName("UnitOfMeasure")]
        public long? ItemUnitOfMeasureId { get; set; }
        public string ItemUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.AdjustQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? AdjustQuantity { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.AdjustUnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? AdjustUnitPrice { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.AdjustCost")]
        [UIHint("DecimalNullable")]
        public decimal? AdjustCost { get; set; }

        [BaseEamResourceDisplayName("AdjustItem.CurrentQuantity")]
        [UIHint("DecimalNullable")]
        public decimal CurrentQuantity { get; set; }

    }
}