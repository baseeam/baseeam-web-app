using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ValueItemCategoryValidator))]
    public class ValueItemCategoryModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("ValueItemCategory.Name")]
        public string Name { get; set; }
    }
}