using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Validators;
using BaseEAM.Web.Models;
using System.Linq;
using FluentValidation;

namespace BaseEAM.Web.Validators
{

    public class TeamValidator : BaseEamValidator<TeamModel>
    {
        private readonly IRepository<Team> _teamRepository;
        public TeamValidator(ILocalizationService localizationService, IRepository<Team> teamRepository)
        {
            this._teamRepository = teamRepository;

            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Team.Name.Required"));
            RuleFor(x => x.SiteId).NotEmpty().WithMessage(localizationService.GetResource("Team.SiteId.Required"));
            RuleFor(x => x).Must(NoDuplication).WithMessage(localizationService.GetResource("Team.Name.Unique"));
        }

        private bool NoDuplication(TeamModel model)
        {
            var team = _teamRepository.GetAll().Where(c => c.Name == model.Name && c.Id != model.Id).FirstOrDefault();
            return team == null;
        }
    }
}