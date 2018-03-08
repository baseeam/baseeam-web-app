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
    [Validator(typeof(SiteValidator))]
    public class SiteModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Site.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Site.Description")]
        public string Description { get; set; }
    }
}