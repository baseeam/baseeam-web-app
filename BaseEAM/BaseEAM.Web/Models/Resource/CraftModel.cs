using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(CraftValidator))]
    public class CraftModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Craft.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Craft.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Craft.StandardRate")]
        [UIHint("DecimalNullable")]
        public decimal? StandardRate { get; set; }

        [BaseEamResourceDisplayName("Craft.OvertimeRate")]
        [UIHint("DecimalNullable")]
        public decimal? OvertimeRate { get; set; }

    }
}