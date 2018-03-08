using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;
using BaseEAM.Services;

namespace BaseEAM.Web.Validators
{
    public class AssignmentGroupValidator : BaseEamValidator<AssignmentGroupModel>
    {
        private readonly IRepository<AssignmentGroup> _assignmentGroupRepository;
        public AssignmentGroupValidator(ILocalizationService localizationService, IRepository<AssignmentGroup> assignmentGroupRepository)
        {
            this._assignmentGroupRepository = assignmentGroupRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AssignmentGroup.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("AssignmentGroup.Name.Unique"));
        }

        private bool NoDuplication(AssignmentGroupModel model)
        {
            var assignmentGroup = _assignmentGroupRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return assignmentGroup == null;
        }
    }
}