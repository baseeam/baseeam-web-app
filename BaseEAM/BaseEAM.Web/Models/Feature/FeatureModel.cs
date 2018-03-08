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
    [Validator(typeof(FeatureValidator))]
    public class FeatureModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Feature.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Feature.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Common.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [BaseEamResourceDisplayName("EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("Feature.WorkflowEnabled")]
        public bool WorkflowEnabled { get; set; }

        [BaseEamResourceDisplayName("Module.Name")]
        public long? ModuleId { get; set; }
    }
}