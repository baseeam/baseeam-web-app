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
    public class WorkOrderLaborValidator : BaseEamValidator<WorkOrderLaborModel>
    {
        public WorkOrderLaborValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.TeamId).NotEmpty().WithMessage(localizationService.GetResource("Team.Required"));
            RuleFor(x => x.TechnicianId).NotEmpty().WithMessage(localizationService.GetResource("Technician.Required"));
            RuleFor(x => x.CraftId).NotEmpty().WithMessage(localizationService.GetResource("Craft.Required"));
        }
    }
}