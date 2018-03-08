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
    [Validator(typeof(SecurityGroupValidator))]
    public class SecurityGroupModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("SecurityGroup.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("SecurityGroup.Description")]
        public string Description { get; set; }
    }
}