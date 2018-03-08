using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ValueItemValidator))]
    public class ValueItemModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("ValueItem.Name")]
        public string Name { get; set; }

        public ValueItemCategoryModel ValueItemCategory { get; set; }
    }
}