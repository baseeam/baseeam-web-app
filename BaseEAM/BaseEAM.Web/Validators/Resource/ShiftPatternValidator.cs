using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{

    public class ShiftPatternValidator : BaseEamValidator<ShiftPatternModel>
    {
        public ShiftPatternValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.StartTime).NotEmpty().WithMessage(localizationService.GetResource("ShiftPattern.StartTime.Required"));
            RuleFor(x => x.EndTime).NotEmpty().WithMessage(localizationService.GetResource("ShiftPattern.EndTime.Required"));
        }
    }
}