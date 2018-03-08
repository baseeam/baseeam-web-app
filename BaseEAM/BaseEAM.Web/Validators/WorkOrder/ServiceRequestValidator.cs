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
    public class ServiceRequestValidator : BaseEamValidator<ServiceRequestModel>
    {
        public ServiceRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.RequestorName).NotEmpty().WithMessage(localizationService.GetResource("ServiceRequest.RequestorName.Required"));
            RuleFor(x => x.RequestorPhone).NotEmpty().WithMessage(localizationService.GetResource("ServiceRequest.RequestorPhone.Required"));
            RuleFor(x => x.RequestorEmail).NotEmpty().WithMessage(localizationService.GetResource("ServiceRequest.RequestorEmail.Required"));
        }
    }
}