using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(MeterLineItemValidator))]
    public class MeterLineItemModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("MeterLineItem.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [BaseEamResourceDisplayName("MeterLineItem.MeterGroup")]
        public long? MeterGroupId { get; set; }
        public MeterGroupModel MeterGroup { get; set; }

        [BaseEamResourceDisplayName("MeterLineItem.Meter")]
        public long? MeterId { get; set; }
        public MeterModel Meter { get; set; }
    }
}