/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ReceiptItemValidator))]
    public class ReceiptItemModel : BaseEamEntityModel
    {
        public ReceiptItemModel()
        {
            AvailableUnitOfMeasures = new List<SelectListItem>();
        }
        public long? ReceiptId { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.StoreLocator")]
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }
        public StoreLocatorModel StoreLocator { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        public long? ItemUnitOfMeasureId { get; set; }
        public string ItemUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("UnitOfMeasure")]
        public long? ReceiptUnitOfMeasureId { get; set; }
        public string ReceiptUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.Quantity")]
        [UIHint("DecimalNullable")]
        public decimal? ReceiptQuantity { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.UnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? ReceiptUnitPrice { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.UnitPrice")]
        [UIHint("DecimalNullable")]
        public decimal? UnitPrice { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.Quantity")]
        [UIHint("DecimalNullable")]
        public decimal? Quantity { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.Cost")]
        [UIHint("DecimalNullable")]
        public decimal? Cost { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.LotNumber")]
        public string LotNumber { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.ExpiryDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? ExpiryDate { get; set; }

        //Cache StoreId from Receipt
        public long? StoreId { get; set; }

        [BaseEamResourceDisplayName("ReceiptItem.CurrentQuantity")]
        [UIHint("DecimalNullable")]
        public decimal CurrentQuantity { get; set; }

        public List<SelectListItem> AvailableUnitOfMeasures { get; set; }
    }
}