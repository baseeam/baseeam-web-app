using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{
    public class MessageTemplateValidator : BaseEamValidator<MessageTemplateModel>
    {
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        public MessageTemplateValidator(ILocalizationService localizationService, IRepository<MessageTemplate> messageTemplateRepository)
        {
            this._messageTemplateRepository = messageTemplateRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("MessageTemplate.Name.Required"));
            RuleFor(x => x.WhereUsed.ToString()).NotEmpty().WithMessage(localizationService.GetResource("MessageTemplate.WhereUsed.Required"));
            RuleFor(x => x.PushTemplate).NotEmpty()
                .When(x => x.IncludesPushNotification).WithMessage(localizationService.GetResource("MessageTemplate.PushTemplate.Required"));
            RuleFor(x => x.SMSTemplate).NotEmpty()
               .When(x => x.IncludesSMS).WithMessage(localizationService.GetResource("MessageTemplate.SMSTemplate.Required"));
            RuleFor(x => x.EmailSubjectTemplate).NotEmpty()
               .When(x => x.IncludesEmail).WithMessage(localizationService.GetResource("MessageTemplate.EmailSubjectTemplate.Required"));
            RuleFor(x => x.EmailBodyTemplate).NotEmpty()
               .When(x => x.IncludesEmail).WithMessage(localizationService.GetResource("MessageTemplate.EmailBodyTemplate.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("MessageTemplate.Name.Unique"));
        }

        private bool NoDuplication(MessageTemplateModel model)
        {
            var messageTemplate = _messageTemplateRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return messageTemplate == null;
        }
    }
}