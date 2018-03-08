/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(StoreLocatorValidator))]
    public class StoreLocatorModel : BaseEamEntityModel
    {
        public long? StoreId { get; set; }

        [BaseEamResourceDisplayName("StoreLocator.Name")]
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}