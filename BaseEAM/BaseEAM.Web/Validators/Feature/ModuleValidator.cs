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
    public class ModuleValidator : BaseEamValidator<ModuleModel>
    {
        private readonly IRepository<Module> _moduleRepository;
        public ModuleValidator(ILocalizationService localizationService, IRepository<Module> moduleRepository)
        {
            this._moduleRepository = moduleRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Module.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Module.Name.Unique"));
            RuleFor(x => x.DisplayOrder).GreaterThan(0).WithMessage(localizationService.GetResource("Common.DisplayOrder.GreaterThanZero"));
        }

        private bool NoDuplication(ModuleModel model)
        {
            var module = _moduleRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return module == null;
        }
    }
}