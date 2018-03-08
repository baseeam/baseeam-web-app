/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Collections.Generic;

namespace BaseEAM.Core.Domain
{
    public class MeterGroup : BaseEntity
    {
        public string Description { get; set; }

        private ICollection<MeterLineItem> _meterLineItems;
        public virtual ICollection<MeterLineItem> MeterLineItems
        {
            get { return _meterLineItems ?? (_meterLineItems = new List<MeterLineItem>()); }
            protected set { _meterLineItems = value; }
        }
    }
}
