/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class Reading : BaseEntity
    {
        public long? PointMeterLineItemId { get; set; }
        public virtual PointMeterLineItem PointMeterLineItem { get; set; }

        public decimal? ReadingValue { get; set; }
        public DateTime? DateOfReading { get; set; }

        public int? ReadingSource { get; set; }

        public long? WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
    }

    public enum ReadingSource
    {
        //input from Asset/Location screen
        Directly = 0,
        //input from WorkOrder
        WorkOrder
    }
}
