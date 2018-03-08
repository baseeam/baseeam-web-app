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
    public class LocationValidator : BaseEamValidator<LocationModel>
    {
        private readonly IRepository<Location> _locationRepository;
        public LocationValidator(ILocalizationService localizationService, IRepository<Location> locationRepository)
        {
            this._locationRepository = locationRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Location.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Location.Name.Unique"));
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x.LocationTypeId).NotEmpty().WithMessage(localizationService.GetResource("Location.LocationType.Required"));
            RuleFor(x => x.LocationStatusId).NotEmpty().WithMessage(localizationService.GetResource("Location.LocationStatus.Required"));
        }

        private bool NoDuplication(LocationModel model)
        {
            var location = _locationRepository.GetAll().Where(c => c.Name == model.Name && c.SiteId == model.SiteId && c.Id != model.Id).FirstOrDefault();
            return location == null;
        }
    }
}