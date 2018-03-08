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
    public class StoreValidator : BaseEamValidator<StoreModel>
    {
        private readonly IRepository<Store> _storeRepository;
        public StoreValidator(ILocalizationService localizationService, IRepository<Store> storeRepository)
        {
            this._storeRepository = storeRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Store.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Store.Name.Unique"));
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.StoreTypeId).NotEmpty().WithMessage(localizationService.GetResource("Store.StoreType.Required"));
        }

        private bool NoDuplication(StoreModel model)
        {
            var store = _storeRepository.GetAll().Where(c => c.Name == model.Name && c.SiteId == model.SiteId && c.Id != model.Id).FirstOrDefault();
            return store == null;
        }
    }
}