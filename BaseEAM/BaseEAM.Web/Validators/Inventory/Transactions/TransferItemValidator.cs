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
    public class TransferItemValidator : BaseEamValidator<TransferItemModel>
    {
        private readonly IRepository<TransferItem> _transferItemRepository;
        private readonly IStoreService _storeService;
        public TransferItemValidator(ILocalizationService localizationService,
            IRepository<TransferItem> transferItemRepository,
            IStoreService storeService)
        {
            this._transferItemRepository = transferItemRepository;
            this._storeService = storeService;

            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("Item.Required"));
            RuleFor(x => x).Must(FromStoreLocatorRequired).WithMessage(localizationService.GetResource("TransferItem.FromStoreLocator.Required"));
            RuleFor(x => x).Must(ToStoreLocatorRequired).WithMessage(localizationService.GetResource("TransferItem.ToStoreLocator.Required"));
            RuleFor(x => x.TransferQuantity).NotEmpty().WithMessage(localizationService.GetResource("TransferItem.TransferQuantity.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("TransferItem.Unique"));
            RuleFor(x => x).Must(HaveLessThanCurrentQuantity).WithMessage(localizationService.GetResource("TransferItem.TransferQuantity.HaveLessThanCurrentQuantity"));
        }

        private bool FromStoreLocatorRequired(TransferItemModel model)
        {
            return (model.FromStoreLocatorId != null && model.FromStoreLocatorId != 0) ||
                        (model.FromStoreLocator != null && model.FromStoreLocator.Id != 0);
        }

        private bool ToStoreLocatorRequired(TransferItemModel model)
        {
            return (model.ToStoreLocatorId != null && model.ToStoreLocatorId != 0) ||
                        (model.ToStoreLocator != null && model.ToStoreLocator.Id != 0);
        }

        private bool HaveLessThanCurrentQuantity(TransferItemModel model)
        {
            var currentQuantity = _storeService.GetTotalQuantity(null, model.FromStoreLocatorId, model.ItemId);
            return currentQuantity >= model.TransferQuantity;
        }

        private bool NoDuplication(TransferItemModel model)
        {
            var transferItem = _transferItemRepository.GetAll()
                .Where(c => c.FromStoreLocatorId == model.FromStoreLocatorId && c.ToStoreLocatorId == model.ToStoreLocatorId && c.ItemId == model.ItemId && c.Id != model.Id && c.TransferId == model.TransferId).FirstOrDefault();
            return transferItem == null;
        }
    }
}