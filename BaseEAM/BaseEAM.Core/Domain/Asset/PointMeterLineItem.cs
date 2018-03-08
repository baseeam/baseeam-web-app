/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class PointMeterLineItem : BaseEntity
    {
        public int DisplayOrder { get; set; }

        public long? PointId { get; set; }
        public virtual Point Point { get; set; }

        public long? MeterId { get; set; }
        public virtual Meter Meter { get; set; }

        private ICollection<Reading> _readings;
        public virtual ICollection<Reading> Readings
        {
            get { return _readings ?? (_readings = new List<Reading>()); }
            protected set { _readings = value; }
        }

        //cache the last reading
        public decimal? LastReadingValue { get; set; }
        public DateTime? LastDateOfReading { get; set; }
        public string LastReadingUser { get; set; }
    }
}
