/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System;
using System.Globalization;

namespace BaseEAM.Web.Validators
{
    public class CurrencyValidator : BaseEamValidator<CurrencyModel>
    {
        public CurrencyValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Currency.Name.Required"));
            RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage(localizationService.GetResource("Currency.CurrencyCode.Required"));
            RuleFor(x => x.Rate).GreaterThan(0).WithMessage(localizationService.GetResource("Currency.Rate.Range"));
            RuleFor(x => x.DisplayLocale)
                .Must(x =>
                {
                    try
                    {
                        if (String.IsNullOrEmpty(x))
                            return true;
                        //let's try to create a CultureInfo object
                        //if "DisplayLocale" is wrong, then exception will be thrown
                        var culture = new CultureInfo(x);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                })
                .WithMessage(localizationService.GetResource("Currency.DisplayLocale.Invalid"));
        }
    }
}