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
    public partial class UserValidator : BaseEamValidator<UserModel>
    {
        public UserValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("User.Name.Required"));
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("User.Email.Required"));
            RuleFor(x => x.LoginName).NotEmpty().WithMessage(localizationService.GetResource("User.LoginName.Required"));
            RuleFor(x => x.LanguageId).NotEmpty().WithMessage(localizationService.GetResource("Language.Required"));
        }
    }
}