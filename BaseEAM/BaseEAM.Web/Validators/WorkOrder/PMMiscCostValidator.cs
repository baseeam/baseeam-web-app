/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class PMMiscCostValidator : BaseEamValidator<PMMiscCostModel>
    {
        private readonly IRepository<PMMiscCost> _pMMiscCostRepository;
        public PMMiscCostValidator(ILocalizationService localizationService, IRepository<PMMiscCost> pMMiscCostRepository)
        {
            this._pMMiscCostRepository = pMMiscCostRepository;

            RuleFor(x => x.Sequence).NotEmpty().WithMessage(localizationService.GetResource("PMMiscCost.Sequence.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("PMMiscCost.Sequence.Unique"));
        }

        private bool NoDuplication(PMMiscCostModel model)
        {
            var pMMiscCost = _pMMiscCostRepository.GetAll().Where(c => c.Sequence == model.Sequence && c.Id != model.Id && c.PreventiveMaintenanceId == model.PreventiveMaintenanceId).FirstOrDefault();
            return pMMiscCost == null;
        }
    }
}