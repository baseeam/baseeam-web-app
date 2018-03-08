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
    public class ValueItemCategoryValidator : BaseEamValidator<ValueItemCategoryModel>
    {
        private readonly IRepository<ValueItemCategory> _valueItemCategoryRepository;
        public ValueItemCategoryValidator(ILocalizationService localizationService, IRepository<ValueItemCategory> valueItemCategoryRepository)
        {
            this._valueItemCategoryRepository = valueItemCategoryRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("ValueItemCategory.Name.Required"));
            //RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ValueItemCategory.Name.Unique"));
        }

        private bool NoDuplication(ValueItemCategoryModel model)
        {
            var valueItemCategory = _valueItemCategoryRepository.GetAll().Where(c => c.Name == model.Name && (c.Id != model.Id)).FirstOrDefault();
            return valueItemCategory == null;
        }
    }
}