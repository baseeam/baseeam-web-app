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
    public class PMItemValidator : BaseEamValidator<PMItemModel>
    {
        public PMItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.StoreId).NotEmpty().WithMessage(localizationService.GetResource("Store.Required"));
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("Item.Required"));
            RuleFor(x => x.StoreLocatorId).NotEmpty().WithMessage(localizationService.GetResource("StoreLocator.Required"));
        }
    }
}