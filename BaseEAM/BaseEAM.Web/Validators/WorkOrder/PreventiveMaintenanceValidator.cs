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
    public class PreventiveMaintenanceValidator : BaseEamValidator<PreventiveMaintenanceModel>
    {
        public PreventiveMaintenanceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("PreventiveMaintenance.Description.Required"));
            RuleFor(x => x.PMStatusId).NotEmpty().WithMessage(localizationService.GetResource("PreventiveMaintenance.PMStatus.Required"));
            RuleFor(x => x).Must(AssetOrLocationRequired).WithMessage(localizationService.GetResource("PreventiveMaintenance.AssetOrLocationRequired"));
            RuleFor(x => x.WorkCategoryId).NotEmpty().WithMessage(localizationService.GetResource("PreventiveMaintenance.WorkCategory.Required"));
            RuleFor(x => x).Must(TimeBasedRequired).WithMessage(localizationService.GetResource("PreventiveMaintenance.TimeBased.Required"));
        }

        private bool AssetOrLocationRequired(PreventiveMaintenanceModel model)
        {
            return model.AssetId != null || model.LocationId != null;
        }

        private bool TimeBasedRequired(PreventiveMaintenanceModel model)
        {
            if (model.FirstWorkExpectedStartDateTime.HasValue)
            {
                return model.FirstWorkDueDateTime.HasValue && model.EndDateTime.HasValue && model.FrequencyCount.HasValue;
            }
            else if (model.FirstWorkDueDateTime.HasValue)
            {
                return model.FirstWorkExpectedStartDateTime.HasValue && model.EndDateTime.HasValue && model.FrequencyCount.HasValue;
            }
            else if (model.EndDateTime.HasValue)
            {
                return model.FirstWorkExpectedStartDateTime.HasValue && model.FirstWorkDueDateTime.HasValue && model.FrequencyCount.HasValue;
            }
            else if (model.FrequencyCount.HasValue)
            {
                return model.FirstWorkExpectedStartDateTime.HasValue && model.FirstWorkDueDateTime.HasValue && model.EndDateTime.HasValue;
            }
            else
            {
                return true;
            }
        }
    }
}