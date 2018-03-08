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
    public class AdjustItemValidator : BaseEamValidator<AdjustItemModel>
    {
        private readonly IRepository<AdjustItem> _adjustItemRepository;
        private readonly IStoreService _storeService;
        public AdjustItemValidator(ILocalizationService localizationService,
            IRepository<AdjustItem> adjustItemRepository,
             IStoreService storeService)
        {
            this._adjustItemRepository = adjustItemRepository;
            this._storeService = storeService;
            RuleFor(x => x).Must(StoreLocatorRequired).WithMessage(localizationService.GetResource("AdjustItem.StoreLocator.Required"));
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("Item.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("AdjustItem.Unique"));
            RuleFor(x => x).Must(HaveUnitPriceIfQuantityGreaterThanCurrentQuantity).WithMessage(localizationService.GetResource("AdjustItem.AdjustUnitPrice.Required"));
            RuleFor(x => x.AdjustQuantity).NotEmpty().WithMessage(localizationService.GetResource("AdjustItem.AdjustQuantity.Required"));
        }

        private bool StoreLocatorRequired(AdjustItemModel model)
        {
            return (model.StoreLocatorId != null && model.StoreLocatorId != 0) ||
                        (model.StoreLocator != null && model.StoreLocator.Id != 0);
        }

        private bool NoDuplication(AdjustItemModel model)
        {
            var adjustItem = _adjustItemRepository.GetAll()
                .Where(c => c.StoreLocatorId == model.StoreLocatorId && c.ItemId == model.ItemId && c.Id != model.Id && c.AdjustId == model.AdjustId).FirstOrDefault();
            return adjustItem == null;
        }

        private bool HaveUnitPriceIfQuantityGreaterThanCurrentQuantity(AdjustItemModel model)
        {
            var currentQuantity = _storeService.GetTotalQuantity(null, model.StoreLocatorId, model.ItemId);

            return currentQuantity < model.AdjustQuantity ? model.AdjustUnitPrice != null : true;
        }
    }
}