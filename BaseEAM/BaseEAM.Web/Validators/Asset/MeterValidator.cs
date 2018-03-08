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

    public class MeterValidator : BaseEamValidator<MeterModel>
    {
        private readonly IRepository<Meter> _meterRepository;
        public MeterValidator(ILocalizationService localizationService, IRepository<Meter> meterRepository)
        {
            this._meterRepository = meterRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Meter.Name.Required"));
            RuleFor(x => x.MeterTypeId).NotEmpty().WithMessage(localizationService.GetResource("Meter.MeterType.Required"));
            RuleFor(x => x.UnitOfMeasureId).NotEmpty().WithMessage(localizationService.GetResource("Meter.UnitOfMeasure.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Meter.Name.Unique"));
        }

        private bool NoDuplication(MeterModel model)
        {
            var meter = _meterRepository.GetAll().Where(c => c.Name == model.Name  && c.Id != model.Id).FirstOrDefault();
            return meter == null;
        }
    }
}