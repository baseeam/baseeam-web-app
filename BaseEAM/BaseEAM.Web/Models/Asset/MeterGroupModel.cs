using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(MeterGroupValidator))]
    public class MeterGroupModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("MeterGroup.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [BaseEamResourceDisplayName("MeterGroup.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("MeterGroup.Description")]
        public string Description { get; set; }
    }
}