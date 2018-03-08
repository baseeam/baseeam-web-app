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
    public class CompanyValidator : BaseEamValidator<CompanyModel>
    {
        private readonly IRepository<Company> _companyRepository;
        public CompanyValidator(ILocalizationService localizationService, IRepository<Company> companyRepository)
        {
            this._companyRepository = companyRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Company.Name.Required"));
            RuleFor(x => x.CompanyTypeId).NotEmpty().WithMessage(localizationService.GetResource("Company.CompanyType.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Company.Name.Unique"));
        }

        private bool NoDuplication(CompanyModel model)
        {
            var company = _companyRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return company == null;
        }
    }
}