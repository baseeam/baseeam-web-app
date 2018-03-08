/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ReceiptValidator))]
    public class ReceiptModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Receipt.Name")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("Receipt.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Receipt.ReceiptDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? ReceiptDate { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("Store")]
        public long? StoreId { get; set; }
        public StoreModel Store { get; set; }

        [BaseEamResourceDisplayName("Receipt.IsApproved")]
        public bool IsApproved { get; set; }

        [BaseEamResourceDisplayName("User")]
        public long? UserId { get; set; }
        public UserModel User { get; set; }
    }
}