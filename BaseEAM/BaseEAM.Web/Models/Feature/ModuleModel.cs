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
    [Validator(typeof(ModuleValidator))]
    public class ModuleModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Module.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Module.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Common.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}