using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AutoNumberValidator))]
    public class AutoNumberModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("AutoNumber.EntityType")]
        public string EntityType { get; set; }

        [BaseEamResourceDisplayName("AutoNumber.FormatString")]
        public string FormatString { get; set; }
    }
}