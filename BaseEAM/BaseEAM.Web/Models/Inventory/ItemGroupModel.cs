using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(ItemGroupValidator))]
    public class ItemGroupModel
    {
        public long Id { get; set; }

        [BaseEamResourceDisplayName("ItemGroup.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("ItemGroup.Description")]
        public string Description { get; set; }

    }
}