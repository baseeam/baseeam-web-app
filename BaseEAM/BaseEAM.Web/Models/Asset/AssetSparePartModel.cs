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
    [Validator(typeof(AssetSparePartValidator))]
    public class AssetSparePartModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Asset")]
        public long? AssetId { get; set; }
        public AssetModel Asset { get; set; }

        [BaseEamResourceDisplayName("Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }

        [BaseEamResourceDisplayName("AssetSparePart.Quantity")]
        [UIHint("DecimalNullable")]
        public decimal? Quantity { get; set; }
    }
}