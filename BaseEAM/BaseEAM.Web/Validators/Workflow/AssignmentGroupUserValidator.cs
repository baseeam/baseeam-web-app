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
    public partial class AssignmentGroupUserValidator : BaseEamValidator<AssignmentGroupUserModel>
    {
        public AssignmentGroupUserValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("AssignmentGroupUser.Site.Required"));
            RuleFor(x => x.UserId).NotEmpty().WithMessage(localizationService.GetResource("AssignmentGroupUser.User.Required"));
        }
    }
}