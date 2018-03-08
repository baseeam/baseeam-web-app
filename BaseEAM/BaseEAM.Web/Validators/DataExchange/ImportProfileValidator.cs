/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using System.Linq;
using FluentValidation;
using BaseEAM.Web.Models;
using BaseEAM.Web.Framework.Validators;

namespace BaseEAM.Web.Validators
{
    public class ImportProfileValidator : BaseEamValidator<ImportProfileModel>
    {
        private readonly IRepository<ImportProfile> _importProfileRepository;
        public ImportProfileValidator(ILocalizationService localizationService,
            IRepository<ImportProfile> importProfileRepository)
        {
            this._importProfileRepository = importProfileRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("ImportProfile.Name.Required"));
            RuleFor(x => x.EntityType).NotEmpty().WithMessage(localizationService.GetResource("EntityType.Required"));
        }

        private bool NoDuplication(ImportProfileModel model)
        {
            var module = _importProfileRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return module == null;
        }
    }
}