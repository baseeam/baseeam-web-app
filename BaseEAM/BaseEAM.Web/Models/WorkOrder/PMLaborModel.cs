/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(PMLaborValidator))]
    public class PMLaborModel : BaseEamEntityModel
    {
        public long? PreventiveMaintenanceId { get; set; }

        [BaseEamResourceDisplayName("Team")]
        public long? TeamId { get; set; }
        public string TeamName { get; set; }

        [BaseEamResourceDisplayName("Technician")]
        public long? TechnicianId { get; set; }
        public string TechnicianName { get; set; }

        [BaseEamResourceDisplayName("Craft")]
        public long? CraftId { get; set; }
        public string CraftName { get; set; }

        [BaseEamResourceDisplayName("PMLabor.PlanHours")]
        [UIHint("DecimalNullable")]
        public decimal? PlanHours { get; set; }

        [BaseEamResourceDisplayName("PMLabor.StandardRate")]
        [UIHint("DecimalNullable")]
        public decimal? StandardRate { get; set; }

        [BaseEamResourceDisplayName("PMLabor.OTRate")]
        [UIHint("DecimalNullable")]
        public decimal? OTRate { get; set; }

        [BaseEamResourceDisplayName("PMLabor.PlanTotal")]
        [UIHint("DecimalNullable")]
        public decimal? PlanTotal { get; set; }

    }
}