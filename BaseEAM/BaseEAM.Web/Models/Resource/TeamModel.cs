using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(TeamValidator))]
    public class TeamModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Team.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Team.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Team.Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

    }
}