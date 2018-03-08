using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class ValueItemValidator : BaseEamValidator<ValueItemModel>
    {
        private readonly IRepository<ValueItem> _valueItemRepository;
        public ValueItemValidator(ILocalizationService localizationService, IRepository<ValueItem> valueItemRepository)
        {
            this._valueItemRepository = valueItemRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("ValueItem.Name.Required"));
            RuleFor(x => x.ValueItemCategory).NotEmpty().WithMessage(localizationService.GetResource("ValueItem.ValueItemCategory.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ValueItem.Name.Unique"));
        }

        private bool NoDuplication(ValueItemModel model)
        {
            ValueItem valueItem;
            if (model.Id == 0)
            {
                valueItem = _valueItemRepository.GetAll().Where(c => c.Name == model.Name && c.ValueItemCategory.Name == model.ValueItemCategory.Name).FirstOrDefault();
            }
            else
            {
                valueItem = _valueItemRepository.GetAll().Where(c => c.Name == model.Name && c.ValueItemCategoryId == model.ValueItemCategory.Id && (c.Id != model.Id)).FirstOrDefault();
            }
            return valueItem == null;
        }
    }
}