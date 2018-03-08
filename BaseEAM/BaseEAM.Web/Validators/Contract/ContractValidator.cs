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
    public class ContractValidator : BaseEamValidator<ContractModel>
    {
        public ContractValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.StartDate).NotEmpty().WithMessage(localizationService.GetResource("Contract.StartDate.Required"));
            RuleFor(x => x.EndDate).NotEmpty().WithMessage(localizationService.GetResource("Contract.EndDate.Required"));
            RuleFor(x => x.Total).NotEmpty().WithMessage(localizationService.GetResource("Contract.Total.Required"));
            RuleFor(x => x.VendorId).NotEmpty().WithMessage(localizationService.GetResource("Vendor.Required"));
        }
    }
}