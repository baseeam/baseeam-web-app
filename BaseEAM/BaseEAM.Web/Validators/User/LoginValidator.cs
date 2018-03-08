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
    public class LoginValidator : BaseEamValidator<LoginModel>
    {
        public LoginValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.LoginName).NotEmpty().WithMessage(localizationService.GetResource("User.LoginName.Required"));
            RuleFor(x => x.LoginPassword).NotEmpty().WithMessage(localizationService.GetResource("User.LoginPassword.Required"));
        }
    }
}