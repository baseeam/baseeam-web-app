using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class CommentValidator : BaseEamValidator<CommentModel>
    {
        public CommentValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Comment.Message.Required"));
        }
    }
}