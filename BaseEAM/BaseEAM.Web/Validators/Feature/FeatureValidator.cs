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
    public class FeatureValidator : BaseEamValidator<FeatureModel>
    {
        private readonly IRepository<Feature> _featureRepository;
        public FeatureValidator(ILocalizationService localizationService, IRepository<Feature> featureRepository)
        {
            this._featureRepository = featureRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Feature.Name.Required"));
            RuleFor(x => x.ModuleId).NotEmpty().WithMessage(localizationService.GetResource("Feature.Module.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Feature.Name.Unique"));
            RuleFor(x => x.DisplayOrder).GreaterThan(0).WithMessage(localizationService.GetResource("Common.DisplayOrder.GreaterThanZero"));
        }

        private bool NoDuplication(FeatureModel model)
        {
            var feature = _featureRepository.GetAll().Where(c => c.Name == model.Name && c.ModuleId == model.ModuleId && c.Id != model.Id).FirstOrDefault();
            return feature == null;
        }
    }
}