using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(CalendarNonWorkingValidator))]
    public class CalendarNonWorkingModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("CalendarNonWorking.Name")]
        public string Name { get; set; }

        [BaseEamResourceDisplayName("CalendarNonWorking.Calendar")]
        public long? CalendarId { get; set; }
        public CalendarModel Calendar { get; set; }

        [BaseEamResourceDisplayName("CalendarNonWorking.NonWorkingDate")]
        [UIHint("DateNullable")]
        public DateTime? NonWorkingDate { get; set; }
    }
}