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

    public class MeterEventValidator : BaseEamValidator<MeterEventModel>
    {
        public MeterEventValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("MeterEvent.Description.Required"));
            RuleFor(x => x).Must(UpperOrLowerLimitRequired).WithMessage(localizationService.GetResource("MeterEvent.UpperOrLowerLimit.Required"));
        }

        private bool UpperOrLowerLimitRequired(MeterEventModel model)
        {
            return model.UpperLimit != null || model.LowerLimit != null;
        }
    }
}