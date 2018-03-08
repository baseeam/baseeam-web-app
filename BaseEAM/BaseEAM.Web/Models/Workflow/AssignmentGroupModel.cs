using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AssignmentGroupValidator))]
    public class AssignmentGroupModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("AssignmentGroup.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("AssignmentGroup.Description")]
        public string Description { get; set; }
    }
}