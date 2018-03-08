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
    public class AssetSparePartValidator : BaseEamValidator<AssetSparePartModel>
    {
        public AssetSparePartValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("Item.Required"));
            RuleFor(x => x.Quantity).NotEmpty().WithMessage(localizationService.GetResource("AssetSparePart.Quantity.Required"));
        }
    }
}