using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class ServiceItemValidator : BaseEamValidator<ServiceItemModel>
    {
        private readonly IRepository<ServiceItem> _serviceItemRepository;
        public ServiceItemValidator(ILocalizationService localizationService, IRepository<ServiceItem> serviceItemRepository)
        {
            this._serviceItemRepository = serviceItemRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("ServiceItem.Name.Required"));
            RuleFor(x => x.ItemGroupId).NotEmpty().WithMessage(localizationService.GetResource("ServiceItem.ItemGroup.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("ServiceItem.Name.Unique"));
        }

        private bool NoDuplication(ServiceItemModel model)
        {
            var serviceItem = _serviceItemRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return serviceItem == null;
        }
    }
}