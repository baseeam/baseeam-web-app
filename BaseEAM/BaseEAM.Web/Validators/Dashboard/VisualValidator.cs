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
    public class VisualValidator : BaseEamValidator<VisualModel>
    {
        private readonly IRepository<Visual> _visualRepository;
        public VisualValidator(ILocalizationService localizationService,
            IRepository<Visual> visualRepository)
        {
            this._visualRepository = visualRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Visual.Name.Required"));
            RuleFor(x => x.XAxis).NotEmpty().When(x => x.VisualType != VisualType.Metric).WithMessage(localizationService.GetResource("Visual.XAxis.Required"));
            RuleFor(x => x.YAxis).NotEmpty().When(x => x.VisualType != VisualType.Metric).WithMessage(localizationService.GetResource("Visual.YAxis.Required"));
            RuleFor(x => x.Query).NotEmpty().When(x => x.IsNew == false).WithMessage(localizationService.GetResource("Visual.Query.Required"));
            RuleFor(x => x.SortExpression).NotEmpty().When(x => x.IsNew == false).WithMessage(localizationService.GetResource("Visual.SortExpression.Required"));
        }

        private bool NoDuplication(VisualModel model)
        {
            var visual = _visualRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return visual == null;
        }
    }
}