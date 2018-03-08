/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Domain;
using BaseEAM.Web.Framework;
using BaseEAM.Web.Framework.Mvc;
using BaseEAM.Web.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseEAM.Web.Models
{
    [Validator(typeof(PointMeterLineItemValidator))]
    public class PointMeterLineItemModel : BaseEamEntityModel
    {
        [BaseEamResourceDisplayName("PointMeterLineItem.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [BaseEamResourceDisplayName("Point")]
        public long? PointId { get; set; }

        [BaseEamResourceDisplayName("Asset")]
        public string PointAssetName { get; set; }

        [BaseEamResourceDisplayName("Location")]
        public string PointLocationName { get; set; }

        [BaseEamResourceDisplayName("Meter")]
        public long? MeterId { get; set; }
        public string Meter { get; set; }

        [BaseEamResourceDisplayName("Meter")]
        public string MeterName { get; set; }

        [BaseEamResourceDisplayName("MeterType")]
        public string MeterMeterTypeName { get; set; }

        [BaseEamResourceDisplayName("UnitOfMeasure")]
        public string MeterUnitOfMeasureName { get; set; }

        [BaseEamResourceDisplayName("PointMeterLineItem.ReadingSource")]
        public ReadingSource ReadingSource { get; set; }
        public string ReadingSourceText { get; set; }

        [BaseEamResourceDisplayName("PointMeterLineItem.ReadingValue")]
        [UIHint("DecimalNullable")]
        public decimal? ReadingValue { get; set; }

        [BaseEamResourceDisplayName("PointMeterLineItem.DateOfReading")]
        [UIHint("DateTimeNullable")]
        public DateTime? DateOfReading { get; set; }

        //cache the last reading
        [BaseEamResourceDisplayName("PointMeterLineItem.LastReadingValue")]
        [UIHint("DecimalNullable")]
        public decimal? LastReadingValue { get; set; }

        [BaseEamResourceDisplayName("PointMeterLineItem.LastDateOfReading")]
        [UIHint("DateTimeNullable")]
        public DateTime? LastDateOfReading { get; set; }

        [BaseEamResourceDisplayName("PointMeterLineItem.LastReadingUser")]
        public string LastReadingUser { get; set; }

        [BaseEamResourceDisplayName("WorkOrder")]
        public long? WorkOrderId { get; set; }
    }
}