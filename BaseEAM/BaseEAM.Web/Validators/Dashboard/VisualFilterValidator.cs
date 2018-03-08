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
    public class VisualFilterValidator : BaseEamValidator<VisualFilterModel>
    {
        private readonly IRepository<VisualFilter> _visualFilterRepository;
        public VisualFilterValidator(ILocalizationService localizationService,
            IRepository<VisualFilter> visualFilterRepository)
        {
            this._visualFilterRepository = visualFilterRepository;
            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage(localizationService.GetResource("VisualFilter.DisplayOrder.Required"));
            RuleFor(x => x.DbColumn).NotEmpty().WithMessage(localizationService.GetResource("VisualFilter.DbColumn.Required"));
            RuleFor(x => x.FilterId).NotEmpty().WithMessage(localizationService.GetResource("Filter.Required"));
            RuleFor(x => x).Must(UniqueDisplayOrder).WithMessage(localizationService.GetResource("VisualFilter.DisplayOrder.Unique"));
        }
        private bool UniqueDisplayOrder(VisualFilterModel model)
        {
            var visualFilter = _visualFilterRepository.GetAll().Where(r => r.DisplayOrder == model.DisplayOrder && r.Id != model.Id).FirstOrDefault();
            return visualFilter == null;
        }
    }
}