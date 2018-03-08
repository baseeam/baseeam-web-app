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
    public class ItemGroupValidator : BaseEamValidator<ItemGroupModel>
    {
        private readonly IRepository<ItemGroup> _itemGroupRepository;
        public ItemGroupValidator(ILocalizationService localizationService, IRepository<ItemGroup> itemGroupRepository)
        {
            this._itemGroupRepository = itemGroupRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("ItemGroup.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ItemGroup.Name.Unique"));
        }

        private bool NoDuplication(ItemGroupModel model)
        {
            var itemGroup = _itemGroupRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return itemGroup == null;
        }
    }
}