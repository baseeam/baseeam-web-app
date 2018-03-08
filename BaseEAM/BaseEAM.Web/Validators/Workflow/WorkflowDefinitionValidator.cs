using System.Linq;
using FluentValidation;
using BaseEAM.Web.Models;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;

namespace BaseEAM.Web.Validators
{
    public class WorkflowDefinitionValidator : BaseEamValidator<WorkflowDefinitionModel>
    {
        private readonly IRepository<WorkflowDefinition> _workflowDefinitionRepository;
        public WorkflowDefinitionValidator(ILocalizationService localizationService, IRepository<WorkflowDefinition> workflowDefinitionRepository)
        {
            this._workflowDefinitionRepository = workflowDefinitionRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("WorkflowDefinition.Name.Required"));
            RuleFor(x => x.EntityType).NotEmpty().WithMessage(localizationService.GetResource("EntityType.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("WorkflowDefinition.Name.Unique"));
            RuleFor(x => x).Must(OnlyOneDefaultForEntityType).WithMessage(localizationService.GetResource("WorkflowDefinition.IsDefault.Unique"));
        }

        private bool NoDuplication(WorkflowDefinitionModel model)
        {
            var workflowDefinition = _workflowDefinitionRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return workflowDefinition == null;
        }

        private bool OnlyOneDefaultForEntityType(WorkflowDefinitionModel model)
        {
            if(model.IsDefault == true)
            {
                var workflowDefinition = _workflowDefinitionRepository.GetAll()
                .Where(c => c.EntityType == model.EntityType && c.IsDefault == true && c.Id != model.Id).FirstOrDefault();
                return workflowDefinition == null;
            }
            return true;
        }
    }
}