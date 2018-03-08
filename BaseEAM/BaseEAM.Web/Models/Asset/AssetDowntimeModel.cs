using BaseEAM.Web.Framework;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(AssetDowntimeValidator))]
    public class AssetDowntimeModel
    {
        public long Id { get; set; }

        public long? AssetId { get; set; }

        [BaseEamResourceDisplayName("AssetDowntime.StartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDateTime { get; set; }

        [BaseEamResourceDisplayName("AssetDowntime.EndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDateTime { get; set; }

        [BaseEamResourceDisplayName("DowntimeType")]
        public long? DowntimeTypeId { get; set; }

        [BaseEamResourceDisplayName("DowntimeType")]
        public string DowntimeTypeName { get; set; }

        [BaseEamResourceDisplayName("AssetDowntime.ReportedDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? ReportedDateTime { get; set; }

        [BaseEamResourceDisplayName("ReportedUser")]
        public long? ReportedUserId { get; set; }

        [BaseEamResourceDisplayName("ReportedUser")]
        public string ReportedUserName { get; set; }
    }
}