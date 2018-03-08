/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class ContractTermValidator : BaseEamValidator<ContractTermModel>
    {
        public ContractTermValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Sequence).NotEmpty().WithMessage(localizationService.GetResource("Common.Sequence.Required"));
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Common.Name.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("Common.Description.Required"));
        }
    }
}