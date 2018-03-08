/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Autofac;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class OrganizationValidator : BaseEamValidator<OrganizationModel>
    {
        private readonly IRepository<Organization> _organizationRepository;
        public OrganizationValidator(ILocalizationService localizationService, IRepository<Organization> organizationRepository)
        {
            this._organizationRepository = organizationRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Organization.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Organization.Name.Unique"));
        }

        private bool NoDuplication(OrganizationModel model)
        {
            var organization = _organizationRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return organization == null;
        }
    }
}