using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class EntityAttributeValidator : BaseEamValidator<EntityAttributeModel>
    {
        public EntityAttributeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.DisplayOrder).NotEmpty().WithMessage(localizationService.GetResource("Attribute.DisplayOrder.Required"));
        }
    }
}