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
    public class FeatureActionValidator : BaseEamValidator<FeatureActionModel>
    {
        private readonly IRepository<FeatureAction> _featureActionRepository;
        public FeatureActionValidator(ILocalizationService localizationService, IRepository<FeatureAction> featureActionRepository)
        {
            this._featureActionRepository = featureActionRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("FeatureAction.Name.Required"));
            RuleFor(x => x.FeatureId).NotEmpty().WithMessage(localizationService.GetResource("FeatureAction.Feature.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("FeatureAction.Name.Unique"));
            RuleFor(x => x.DisplayOrder).GreaterThan(0).WithMessage(localizationService.GetResource("Common.DisplayOrder.GreaterThanZero"));
        }

        private bool NoDuplication(FeatureActionModel model)
        {
            var featureAction = _featureActionRepository.GetAll().Where(c => c.Name == model.Name && c.FeatureId == model.FeatureId && c.Id != model.Id).FirstOrDefault();
            return featureAction == null;
        }
    }
}