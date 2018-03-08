using FluentValidation;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using BaseEAM.Services;

namespace BaseEAM.Web.Validators
{
    public class AssetDowntimeValidator : BaseEamValidator<AssetDowntimeModel>
    {
        public AssetDowntimeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.DowntimeTypeId).NotEmpty().WithMessage(localizationService.GetResource("AssetDowntime.DowntimeType.Required"));
            RuleFor(x => x.StartDateTime).NotEmpty().WithMessage(localizationService.GetResource("AssetDowntime.StartDateTime.Required"));
            RuleFor(x => x.EndDateTime).NotEmpty().WithMessage(localizationService.GetResource("AssetDowntime.EndDateTime.Required"));
            RuleFor(x => x.ReportedDateTime).NotEmpty().WithMessage(localizationService.GetResource("AssetDowntime.ReportedDateTime.Required"));
            RuleFor(x => x.ReportedUserId).NotEmpty().WithMessage(localizationService.GetResource("AssetDowntime.ReportedUserId.Required"));

        }
    }
}