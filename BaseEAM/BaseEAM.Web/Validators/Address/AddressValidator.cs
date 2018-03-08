/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;

namespace BaseEAM.Web.Validators
{
    public class AddressValidator : BaseEamValidator<AddressModel>
    {
        public AddressValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.Address1).NotEmpty().WithMessage(localizationService.GetResource("Address.Address1.Required"));
            //RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(localizationService.GetResource("Address.PhoneNumber.Required"));
            //RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Address.Email.Invalid"));
        }
    }
}