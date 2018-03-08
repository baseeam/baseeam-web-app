/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class PMMeterFrequencyValidator : BaseEamValidator<PMMeterFrequencyModel>
    {
        private readonly IRepository<PMMeterFrequency> _pMMeterFrequencyRepository;
        public PMMeterFrequencyValidator(ILocalizationService localizationService, IRepository<PMMeterFrequency> pMMeterFrequencyRepository)
        {
            this._pMMeterFrequencyRepository = pMMeterFrequencyRepository;
            RuleFor(x => x.MeterId).NotEmpty().WithMessage(localizationService.GetResource("Meter.Required"));
            RuleFor(x => x.Frequency).NotEmpty().WithMessage(localizationService.GetResource("PMMeterFrequency.Frequency.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Meter.Unique"));
        }

        private bool NoDuplication(PMMeterFrequencyModel model)
        {
            var pMMeterFrequency = _pMMeterFrequencyRepository.GetAll().Where(c => c.MeterId == model.MeterId && c.Id != model.Id && c.PreventiveMaintenanceId == model.PreventiveMaintenanceId).FirstOrDefault();
            return pMMeterFrequency == null;
        }
    }
}