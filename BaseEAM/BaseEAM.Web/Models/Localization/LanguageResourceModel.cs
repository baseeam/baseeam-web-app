/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Web.Mvc;
using FluentValidation.Attributes;
using BaseEAM.Web.Validators;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(LanguageResourceValidator))]
    public partial class LanguageResourceModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("LanguageResource.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("LanguageResource.Value")]
        [AllowHtml]
        public string Value { get; set; }

        public long LanguageId { get; set; }
    }
}