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
    public class WorkOrderServiceItemValidator : BaseEamValidator<WorkOrderServiceItemModel>
    {
        private readonly IRepository<WorkOrderServiceItem> _workOrderServiceItemRepository;
        public WorkOrderServiceItemValidator(ILocalizationService localizationService, IRepository<WorkOrderServiceItem> workOrderServiceItemRepository)
        {
            this._workOrderServiceItemRepository = workOrderServiceItemRepository;
            RuleFor(x => x.ServiceItemId).NotEmpty().WithMessage(localizationService.GetResource("ServiceItem.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ServiceItem.Unique"));
        }

        private bool NoDuplication(WorkOrderServiceItemModel model)
        {
            var workOrderServiceItem = _workOrderServiceItemRepository.GetAll().Where(c => c.ServiceItemId == model.ServiceItemId && c.Id != model.Id).FirstOrDefault();
            return workOrderServiceItem == null;
        }
    }
}