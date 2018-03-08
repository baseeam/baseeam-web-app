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
    public class PMServiceItemValidator : BaseEamValidator<PMServiceItemModel>
    {
        private readonly IRepository<PMServiceItem> _pMServiceItemRepository;
        public PMServiceItemValidator(ILocalizationService localizationService, IRepository<PMServiceItem> pMServiceItemRepository)
        {
            this._pMServiceItemRepository = pMServiceItemRepository;
            RuleFor(x => x.ServiceItemId).NotEmpty().WithMessage(localizationService.GetResource("ServiceItem.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ServiceItem.Unique"));
        }

        private bool NoDuplication(PMServiceItemModel model)
        {
            var pMServiceItem = _pMServiceItemRepository.GetAll().Where(c => c.ServiceItemId == model.ServiceItemId && c.Id != model.Id).FirstOrDefault();
            return pMServiceItem == null;
        }
    }
}