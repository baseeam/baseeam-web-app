/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class Point : BaseEntity
    {
        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }

        public long? AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public long? MeterGroupId { get; set; }
        public virtual MeterGroup MeterGroup { get; set; }

        private ICollection<PointMeterLineItem> _pointMeterLineItems;
        public virtual ICollection<PointMeterLineItem> PointMeterLineItems
        {
            get { return _pointMeterLineItems ?? (_pointMeterLineItems = new List<PointMeterLineItem>()); }
            protected set { _pointMeterLineItems = value; }
        }        
    }
}
