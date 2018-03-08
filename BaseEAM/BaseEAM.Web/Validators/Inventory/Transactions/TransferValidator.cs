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
    public class TransferValidator : BaseEamValidator<TransferModel>
    {
        public TransferValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.TransferDate).NotEmpty().WithMessage(localizationService.GetResource("Transfer.TransferDate.Required"));
            RuleFor(x => x.FromSiteId).NotEmpty().WithMessage(localizationService.GetResource("Transfer.FromSite.Required"));
            RuleFor(x => x.FromStoreId).NotEmpty().WithMessage(localizationService.GetResource("Transfer.FromStore.Required"));
            RuleFor(x => x.ToSiteId).NotEmpty().WithMessage(localizationService.GetResource("Transfer.ToSite.Required"));
            RuleFor(x => x.ToStoreId).NotEmpty().WithMessage(localizationService.GetResource("Transfer.ToStore.Required"));
        }
    }
}