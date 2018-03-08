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
    public class SecurityGroupValidator : BaseEamValidator<SecurityGroupModel>
    {
        private readonly IRepository<SecurityGroup> _securityGroupRepository;
        public SecurityGroupValidator(ILocalizationService localizationService, IRepository<SecurityGroup> securityGroupRepository)
        {
            this._securityGroupRepository = securityGroupRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("SecurityGroup.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("SecurityGroup.Name.Unique"));
        }

        private bool NoDuplication(SecurityGroupModel model)
        {
            var securityGroup = _securityGroupRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return securityGroup == null;
        }
    }
}