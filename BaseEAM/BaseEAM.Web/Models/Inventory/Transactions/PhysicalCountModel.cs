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
    [Validator(typeof(PhysicalCountValidator))]
    public class PhysicalCountModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("PhysicalCount.Number")]
        public string Number { get; set; }

        [BaseEamResourceDisplayName("PhysicalCount.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("PhysicalCount.PhysicalCountDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? PhysicalCountDate { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("Store")]
        public long? StoreId { get; set; }
        public StoreModel Store { get; set; }

        [BaseEamResourceDisplayName("PhysicalCount.IsApproved")]
        public bool IsApproved { get; set; }

    }
}