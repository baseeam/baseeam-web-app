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
    [Validator(typeof(FeatureActionValidator))]
    public class FeatureActionModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Feature.Name")]
        public long? FeatureId { get; set; }

        [BaseEamResourceDisplayName("FeatureAction.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("FeatureAction.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Common.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}