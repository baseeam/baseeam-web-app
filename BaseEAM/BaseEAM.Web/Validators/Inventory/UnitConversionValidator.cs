using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class UnitConversionValidator : BaseEamValidator<UnitConversionModel>
    {
        private readonly IRepository<UnitConversion> _unitConversionRepository;
        public UnitConversionValidator(ILocalizationService localizationService, IRepository<UnitConversion> unitConversionRepository)
        {
            this._unitConversionRepository = unitConversionRepository;
            RuleFor(x => x.FromUnitOfMeasure).NotNull().WithMessage(localizationService.GetResource("UnitConversion.FromUnitOfMeasure.Required"));
            RuleFor(x => x.ToUnitOfMeasure).NotNull().WithMessage(localizationService.GetResource("UnitConversion.ToUnitOfMeasure.Required"));
            RuleFor(x => x.ConversionFactor).NotEmpty().WithMessage(localizationService.GetResource("UnitConversion.ConversionFactor.Required"));
        }
    }
}