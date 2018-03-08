/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class MeterEvent : BaseEntity
    {
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public long? PointId { get; set; }
        public virtual Point Point { get; set; }

        public long? MeterId { get; set; }
        public virtual Meter Meter { get; set; }

        public decimal? UpperLimit { get; set; }
        public decimal? LowerLimit { get; set; }

        private ICollection<MeterEventHistory> _meterEventHistories;
        public virtual ICollection<MeterEventHistory> MeterEventHistories
        {
            get { return _meterEventHistories ?? (_meterEventHistories = new List<MeterEventHistory>()); }
            protected set { _meterEventHistories = value; }
        }
    }
}
