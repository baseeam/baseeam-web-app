/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using System.Linq;
using FluentValidation;
using BaseEAM.Web.Models;
using BaseEAM.Web.Framework.Validators;

namespace BaseEAM.Web.Validators
{
    public class AuditEntityConfigurationValidator : BaseEamValidator<AuditEntityConfigurationModel>
    {
        private readonly IRepository<AuditEntityConfiguration> _auditEntityConfigurationRepository;
        public AuditEntityConfigurationValidator(ILocalizationService localizationService,
            IRepository<AuditEntityConfiguration> auditEntityConfigurationRepository)
        {
            this._auditEntityConfigurationRepository = auditEntityConfigurationRepository;

            RuleFor(x => x.EntityType).NotEmpty().WithMessage(localizationService.GetResource("EntityType.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("EntityType.Unique"));
        }

        private bool NoDuplication(AuditEntityConfigurationModel model)
        {
            var auditEntityConfiguration = _auditEntityConfigurationRepository.GetAll().Where(c => c.EntityType == model.EntityType && c.Id != model.Id).FirstOrDefault();
            return auditEntityConfiguration == null;
        }
    }
}