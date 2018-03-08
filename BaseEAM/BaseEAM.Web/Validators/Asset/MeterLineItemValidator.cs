using FluentValidation;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using BaseEAM.Services;

namespace BaseEAM.Web.Validators
{
    public class MeterLineItemValidator : BaseEamValidator<MeterLineItemModel>
    {
        public MeterLineItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage(localizationService.GetResource("MeterLineItem.DisplayOrder.Required"));
        }
    }
}