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
    public class ReportFilterValidator : BaseEamValidator<ReportFilterModel>
    {
        private readonly IRepository<ReportFilter> _reportFilterRepository;
        public ReportFilterValidator(ILocalizationService localizationService,
            IRepository<ReportFilter> reportFilterRepository)
        {
            this._reportFilterRepository = reportFilterRepository;
            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage(localizationService.GetResource("ReportFilter.DisplayOrder.Required"));
            RuleFor(x => x.DbColumn).NotEmpty().WithMessage(localizationService.GetResource("ReportFilter.DbColumn.Required"));
            RuleFor(x => x.FilterId).NotEmpty().WithMessage(localizationService.GetResource("Filter.Required"));
            RuleFor(x => x).Must(UniqueDisplayOrder).WithMessage(localizationService.GetResource("ReportFilter.DisplayOrder.Unique"));
        }
        private bool UniqueDisplayOrder(ReportFilterModel model)
        {
            var reportFilter = _reportFilterRepository.GetAll()
                .Where(r => r.DisplayOrder == model.DisplayOrder && r.ReportId == model.ReportId && r.Id != model.Id).FirstOrDefault();
            return reportFilter == null;
        }
    }
}