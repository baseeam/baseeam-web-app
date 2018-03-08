using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;
using System.Linq;

namespace BaseEAM.Web.Validators
{
    public class AssetValidator : BaseEamValidator<AssetModel>
    {
        private readonly IRepository<Asset> _assetRepository;
        public AssetValidator(ILocalizationService localizationService, IRepository<Asset> assetRepository)
        {
            this._assetRepository = assetRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Asset.Name.Required"));
            RuleFor(x => x.AssetTypeId).NotEmpty().WithMessage(localizationService.GetResource("Asset.AssetType.Required"));
            RuleFor(x => x.AssetStatusId).NotEmpty().WithMessage(localizationService.GetResource("Asset.AssetStatus.Required"));
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Site.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Asset.Name.Unique"));
        }

        private bool NoDuplication(AssetModel model)
        {
            var asset = _assetRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return asset == null;
        }
    }
}