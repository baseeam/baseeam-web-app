using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(CalendarValidator))]
    public class CalendarModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("Calendar.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("Calendar.Description")]
        public string Description { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsSunday")]
        public bool IsSunday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsMonday")]
        public bool IsMonday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsTuesday")]
        public bool IsTuesday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsWednesday")]
        public bool IsWednesday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsThursday")]
        public bool IsThursday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsFriday")]
        public bool IsFriday { get; set; }

        [BaseEamResourceDisplayName("Calendar.IsSaturday")]
        public bool IsSaturday { get; set; }

    }
}