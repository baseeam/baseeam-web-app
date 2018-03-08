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
    [Validator(typeof(IssueItemValidator))]
    public class IssueItemModel : BaseEamEntityModel
    {
        public IssueItemModel()
        {
            AvailableUnitOfMeasures = new List<SelectListItem>();
        }

        //Cache StoreId from Issue
        public long? StoreId { get; set; }

        public long? IssueId { get; set; }

        [BaseEamResourceDisplayName("Issue.Number")]
        public string IssueNumber { get; set; }

        [BaseEamResourceDisplayName("Issue.IssueDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? IssueDate { get; set; }

        [BaseEamResourceDisplayName("IssueItem.StoreLocator")]
        public long? StoreLocatorId { get; set; }
        public string StoreLocatorName { get; set; }
        public StoreLocatorModel StoreLocator { get; set; }

        [BaseEamResourceDisplayName("IssueItem.Item")]
        public long? ItemId { get; set; }
        public string ItemName { get; set; }
        [BaseEamResourceDisplayName("IssueItem.Item")]
        public long? ItemUnitOfMeasureId { get; set; }
        public string ItemUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("IssueItem.IssueQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? IssueQuantity { get; set; }

        public decimal? Quantity { get; set; }

        [BaseEamResourceDisplayName("UnitOfMeasure")]
        public long? IssueUnitOfMeasureId { get; set; }
        public string IssueUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("IssueItem.IssueCost")]
        [UIHint("DecimalNullable")]
        public decimal? IssueCost { get; set; }

        [BaseEamResourceDisplayName("IssueItem.CurrentQuantity")]
        [UIHint("DecimalNullable")]
        public decimal CurrentQuantity { get; set; }

        public List<SelectListItem> AvailableUnitOfMeasures { get; set; }
    }
}