/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core.Domain
{
    public class WorkOrderMiscCost : BaseEntity, ISyncEntity
    {
        public WorkOrderMiscCost()
        {
            SyncId = Guid.NewGuid().ToString();
        }
        public string SyncId { get; set; }

        public long? WorkOrderId { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        public int? Sequence { get; set; }
        public string Description { get; set; }

        public decimal? PlanUnitPrice { get; set; }
        public decimal? PlanQuantity { get; set; }
        public decimal? PlanTotal { get; set; }

        public decimal? ActualUnitPrice { get; set; }
        public decimal? ActualQuantity { get; set; }
        public decimal? ActualTotal { get; set; }
    }
}
