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
    public class ReportValidator : BaseEamValidator<ReportModel>
    {
        private readonly IRepository<Report> _reportRepository;
        public ReportValidator(ILocalizationService localizationService,
            IRepository<Report> reportRepository)
        {
            this._reportRepository = reportRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Report.Name.Required"));
            RuleFor(x => x.Type).NotEmpty().WithMessage(localizationService.GetResource("Report.Type.Required"));
            RuleFor(x => x.Query).NotEmpty().When(x => x.IsNew == false).WithMessage(localizationService.GetResource("Report.Query.Required"));
            RuleFor(x => x.SortExpression).NotEmpty().When(x => x.IsNew == false).WithMessage(localizationService.GetResource("Report.SortExpression.Required"));
        }

        private bool NoDuplication(ReportModel model)
        {
            var report = _reportRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return report == null;
        }
    }
}