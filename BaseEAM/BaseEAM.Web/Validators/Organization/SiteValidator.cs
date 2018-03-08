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
    public class SiteValidator : BaseEamValidator<SiteModel>
    {
        private readonly IRepository<Site> _siteRepository;
        public SiteValidator(ILocalizationService localizationService, IRepository<Site> siteRepository)
        {
            this._siteRepository = siteRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Site.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Site.Name.Unique"));
        }

        private bool NoDuplication(SiteModel model)
        {
            var site = _siteRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return site == null;
        }
    }
}