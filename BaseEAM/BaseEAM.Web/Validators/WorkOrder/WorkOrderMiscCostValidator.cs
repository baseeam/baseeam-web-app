/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class WorkOrderMiscCostValidator : BaseEamValidator<WorkOrderMiscCostModel>
    {
        private readonly IRepository<WorkOrderMiscCost> _workOrderMiscCostRepository;
        public WorkOrderMiscCostValidator(ILocalizationService localizationService, IRepository<WorkOrderMiscCost> workOrderMiscCostRepository)
        {
            this._workOrderMiscCostRepository = workOrderMiscCostRepository;

            RuleFor(x => x.Sequence).NotEmpty().WithMessage(localizationService.GetResource("WorkOrderMiscCost.Sequence.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("WorkOrderMiscCost.Sequence.Unique"));
        }

        private bool NoDuplication(WorkOrderMiscCostModel model)
        {
            var workOrderMiscCost = _workOrderMiscCostRepository.GetAll().Where(c => c.Sequence == model.Sequence && c.Id != model.Id && c.WorkOrderId == model.WorkOrderId).FirstOrDefault();
            return workOrderMiscCost == null;
        }
    }
}