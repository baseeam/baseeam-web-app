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
    public class ReportColumnValidator : BaseEamValidator<ReportColumnModel>
    {
        public ReportColumnValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ColumnName).NotEmpty().WithMessage(localizationService.GetResource("ReportColumn.ColumnName.Required"));
            RuleFor(x => x.ResourceKey).NotEmpty().WithMessage(localizationService.GetResource("ReportColumn.ResourceKey.Required"));
        }

    }
}