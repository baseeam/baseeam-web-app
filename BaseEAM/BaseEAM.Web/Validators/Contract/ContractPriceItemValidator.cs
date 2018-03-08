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
    public class ContractPriceItemValidator : BaseEamValidator<ContractPriceItemModel>
    {
        private readonly IRepository<ContractPriceItem> _contractPriceItemRepository;
        public ContractPriceItemValidator(ILocalizationService localizationService, IRepository<ContractPriceItem> contractPriceItemRepository)
        {
            this._contractPriceItemRepository = contractPriceItemRepository;

            RuleFor(x => x.ItemId).NotEmpty().WithMessage(localizationService.GetResource("Item.Required"));
            RuleFor(x => x.ContractedPrice).NotEmpty().WithMessage(localizationService.GetResource("ContractPriceItem.ContractedPrice.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Item.Unique"));
        }

        private bool NoDuplication(ContractPriceItemModel model)
        {
            var contractPriceItem = _contractPriceItemRepository.GetAll()
                .Where(c => c.ItemId == model.ItemId && c.Id != model.Id && c.ContractId == model.ContractId).FirstOrDefault();
            return contractPriceItem == null;
        }
    }
}