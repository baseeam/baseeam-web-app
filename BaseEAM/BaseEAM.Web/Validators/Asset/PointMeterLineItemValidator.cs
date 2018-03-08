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
    public class PointMeterLineItemValidator : BaseEamValidator<PointMeterLineItemModel>
    {
        private readonly IRepository<PointMeterLineItem> _pointMeterLineItemRepository;
        public PointMeterLineItemValidator(ILocalizationService localizationService,
            IRepository<PointMeterLineItem> pointMeterLineItemRepository)
        {
            this._pointMeterLineItemRepository = pointMeterLineItemRepository;
            RuleFor(x => x.MeterId).NotEmpty().WithMessage(localizationService.GetResource("PointMeterLineItem.Meter.Required"));
            RuleFor(x => x.ReadingValue).NotEmpty()
                .When(x => x.DateOfReading.HasValue).WithMessage(localizationService.GetResource("PointMeterLineItem.ReadingValue.Required"));
            RuleFor(x => x.DateOfReading).NotEmpty()
                .When(x => x.ReadingValue.HasValue).WithMessage(localizationService.GetResource("PointMeterLineItem.DateOfReading.Required"));
            RuleFor(x => x).Must(UniqueMeter).WithMessage(localizationService.GetResource("Meter.Name.Unique"));

        }

        private bool UniqueMeter(PointMeterLineItemModel model)
        {
            var pointMeterLineItem = _pointMeterLineItemRepository.GetAll().Where(c => c.MeterId == model.MeterId && c.Id != model.Id && c.PointId == model.PointId).FirstOrDefault();
            return pointMeterLineItem == null;
        }
    }
}