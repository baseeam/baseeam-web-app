/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ReturnItemValidator))]
    [Bind(Exclude = "IssueItem")]
    public class ReturnItemModel
    {
        public long Id { get; set; }

        public long? ReturnId { get; set; }

        [BaseEamResourceDisplayName("IssueItem")]
        public long? IssueItemId { get; set; }
        public IssueItemModel IssueItem { get; set; }

        [BaseEamResourceDisplayName("ReturnItem.ReturnQuantity")]
        [UIHint("DecimalNullable")]
        public decimal? ReturnQuantity { get; set; }

        [BaseEamResourceDisplayName("ReturnItem.ReturnCost")]
        [UIHint("DecimalNullable")]
        public decimal? ReturnCost { get; set; }

    }
}