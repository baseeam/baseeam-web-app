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
    public class WorkOrderValidator : BaseEamValidator<WorkOrderModel>
    {
        public WorkOrderValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("WorkOrder.Description.Required"));
            RuleFor(x => x).Must(AssetOrLocationRequired).WithMessage(localizationService.GetResource("WorkOrder.AssetOrLocationRequired"));
        }

        private bool AssetOrLocationRequired(WorkOrderModel model)
        {
            return model.AssetId != null || model.LocationId != null;
        }
    }
}