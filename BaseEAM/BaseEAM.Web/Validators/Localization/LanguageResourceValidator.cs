/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using FluentValidation;
using BaseEAM.Web.Models;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;

namespace BaseEAM.Web.Validators
{
    public partial class LanguageResourceValidator : BaseEamValidator<LanguageResourceModel>
    {
        public LanguageResourceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("LanguageResource.Name.Required"));
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResource("LanguageResource.Value.Required"));
        }
    }
}