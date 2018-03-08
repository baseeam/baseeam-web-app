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
    public class PhysicalCountItemValidator : BaseEamValidator<PhysicalCountItemModel>
    {
        public PhysicalCountItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Count).NotEmpty().WithMessage(localizationService.GetResource("PhysicalCountItem.Count.Required"));
        }
    }
}