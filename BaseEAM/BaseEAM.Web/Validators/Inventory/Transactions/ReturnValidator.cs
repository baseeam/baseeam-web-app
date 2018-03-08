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
    public class ReturnValidator : BaseEamValidator<ReturnModel>
    {
        public ReturnValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.StoreId).NotEmpty().WithMessage(localizationService.GetResource("Store.Required"));
            RuleFor(x => x.ReturnDate).NotEmpty().WithMessage(localizationService.GetResource("Return.ReturnDate.Required"));
        }
    }
}