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
    public class ReceiptItemValidator : BaseEamValidator<ReceiptItemModel>
    {
        private readonly IRepository<ReceiptItem> _receiptItemRepository;
        public ReceiptItemValidator(ILocalizationService localizationService, IRepository<ReceiptItem> receiptItemRepository)
        {
            this._receiptItemRepository = receiptItemRepository;

            RuleFor(x => x).Must(StoreLocatorRequired).WithMessage(localizationService.GetResource("ReceiptItem.StoreLocator.Required"));
            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("ReceiptItem.Item.Required"));
            RuleFor(x => x.ReceiptQuantity).NotEmpty().WithMessage(localizationService.GetResource("ReceiptItem.Quantity.Required"));
            RuleFor(x => x.ReceiptUnitPrice).NotEmpty().WithMessage(localizationService.GetResource("ReceiptItem.UnitPrice.Required"));
            RuleFor(x => x.ReceiptUnitOfMeasureId).NotEmpty().WithMessage(localizationService.GetResource("UnitOfMeasure.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ReceiptItem.Unique"));
        }

        private bool StoreLocatorRequired(ReceiptItemModel model)
        {
            return (model.StoreLocatorId != null && model.StoreLocatorId != 0) ||
                        (model.StoreLocator != null && model.StoreLocator.Id != 0);
        }

        private bool NoDuplication(ReceiptItemModel model)
        {
            var receiptItem = _receiptItemRepository.GetAll()
                .Where(c => c.StoreLocatorId == model.StoreLocatorId && c.ItemId == model.ItemId && c.Id != model.Id && c.ReceiptId == model.ReceiptId).FirstOrDefault();
            return receiptItem == null;
        }
    }
}