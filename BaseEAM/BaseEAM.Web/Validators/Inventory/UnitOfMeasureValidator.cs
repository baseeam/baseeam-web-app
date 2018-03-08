using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class UnitOfMeasureValidator : BaseEamValidator<UnitOfMeasureModel>
    {
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;

        public UnitOfMeasureValidator(ILocalizationService localizationService, IRepository<UnitOfMeasure> unitOfMeasureRepository)
        {
            this._unitOfMeasureRepository = unitOfMeasureRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("UnitOfMeasure.Name.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("UnitOfMeasure.Name.Unique"));
        }

        private bool NoDuplication(UnitOfMeasureModel model)
        {
            var unitOfMeasure = _unitOfMeasureRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return unitOfMeasure == null;
        }
    }
}