using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{

    public class AutoNumberValidator : BaseEamValidator<AutoNumberModel>
    {
        private readonly IRepository<AutoNumber> _autoNumberRepository;
        public AutoNumberValidator(ILocalizationService localizationService, IRepository<AutoNumber> autoNumberRepository)
        {
            this._autoNumberRepository = autoNumberRepository;

            RuleFor(x => x.EntityType).NotEmpty().WithMessage(localizationService.GetResource("AutoNumber.EntityType.Required"));
            RuleFor(x => x.FormatString).NotEmpty().WithMessage(localizationService.GetResource("AutoNumber.FormatString.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("AutoNumber.EntityType.Unique"));
        }

        private bool NoDuplication(AutoNumberModel model)
        {
            var autoNumber = _autoNumberRepository.GetAll().Where(c => c.EntityType == model.EntityType && c.Id != model.Id).FirstOrDefault();
            return autoNumber == null;
        }
    }
}