/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using BaseEAM.Web.Validators;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(LanguageValidator))]
    public partial class LanguageModel : BaseEamEntityModel
    {
        public LanguageModel()
        {
        }

        [BaseEamResourceDisplayName("Language.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Language.LanguageCulture")]
        [AllowHtml]
        public string LanguageCulture { get; set; }

        [BaseEamResourceDisplayName("Language.FlagImageFileName")]
        public string FlagImageFileName { get; set; }

        [BaseEamResourceDisplayName("Language.Published")]
        public bool Published { get; set; }

        [BaseEamResourceDisplayName("Common.DisplayOrder")]
        public int DisplayOrder { get; set; }

        // search
        public LanguageResourcesListModel Search { get; set; }
    }
}