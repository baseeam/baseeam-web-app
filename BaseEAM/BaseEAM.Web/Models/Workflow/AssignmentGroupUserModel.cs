using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AssignmentGroupUserValidator))]
    public class AssignmentGroupUserModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("AssignmentGroupUser.Name")]
        public long? AssignmentGroupId { get; set; }

        [BaseEamResourceDisplayName("Site")]
        public long? SiteId { get; set; }
        public SiteModel Site { get; set; }

        [BaseEamResourceDisplayName("User")]
        public long? UserId { get; set; }
        public UserModel User { get; set; }

        [BaseEamResourceDisplayName("AssignmentGroupUser.IsDefaultUser")]
        public bool IsDefaultUser { get; set; }

    }
}