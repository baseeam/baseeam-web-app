using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class CraftValidator : BaseEamValidator<CraftModel>
    {
        private readonly IRepository<Craft> _craftRepository;
        public CraftValidator(ILocalizationService localizationService, IRepository<Craft> craftRepository)
        {
            this._craftRepository = craftRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Craft.Name.Required"));
            RuleFor(x => x.StandardRate).NotEmpty().WithMessage(localizationService.GetResource("Craft.StandardRate.Required"));
            RuleFor(x => x.OvertimeRate).NotEmpty().WithMessage(localizationService.GetResource("Craft.OvertimeRate.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Craft.Name.Unique"));
        }

        private bool NoDuplication(CraftModel model)
        {
            var craft = _craftRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return craft == null;
        }
    }
}