using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(TechnicianValidator))]
    public class TechnicianModel : BaseEamEntityModel
    {

        [BaseEamResourceDisplayName("Technician.UserName")]
        public long? UserId { get; set; }
        public UserModel User { get; set; }

        [BaseEamResourceDisplayName("Technician.CalendarName")]
        public long? CalendarId { get; set; }
        public CalendarModel Calendar { get; set; }

        [BaseEamResourceDisplayName("Technician.ShiftName")]
        public long? ShiftId { get; set; }
        public ShiftModel Shift { get; set; }

        [BaseEamResourceDisplayName("Technician.CraftName")]
        public long? CraftId { get; set; }
        public CraftModel Craft { get; set; }
    }
}