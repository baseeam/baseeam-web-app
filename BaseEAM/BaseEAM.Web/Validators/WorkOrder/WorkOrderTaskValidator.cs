using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class WorkOrderTaskValidator : BaseEamValidator<WorkOrderTaskModel>
    {
        private readonly IRepository<WorkOrderTask> _workOrderTaskRepository;
        public WorkOrderTaskValidator(ILocalizationService localizationService, IRepository<WorkOrderTask> workOrderTaskRepository)
        {
            this._workOrderTaskRepository = workOrderTaskRepository;

            RuleFor(x => x.Sequence).NotEmpty().WithMessage(localizationService.GetResource("WorkOrderTask.Sequence.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("WorkOrderTask.Description.Required"));
            RuleFor(x => x.CompletedUserId).NotEmpty()
                .When(x => x.Completed).WithMessage(localizationService.GetResource("WorkOrderTask.CompletedUser.Required"));
            RuleFor(x => x.CompletedDate).NotEmpty()
                .When(x => x.Completed).WithMessage(localizationService.GetResource("WorkOrderTask.CompletedDate.Required"));

            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("WorkOrderTask.Sequence.Unique"));
        }

        private bool NoDuplication(WorkOrderTaskModel model)
        {
            var workOrderTask = _workOrderTaskRepository.GetAll().Where(c => c.Sequence == model.Sequence && c.Id != model.Id && c.WorkOrderId == model.WorkOrderId).FirstOrDefault();
            return workOrderTask == null;
        }
    }
}